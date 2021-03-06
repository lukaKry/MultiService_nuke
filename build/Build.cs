using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    // w tym miejscu jest punkt startowy projektu build
    // je?li wpiszemy x.Compile, to pierwszy odpali si? target o nazwie Compile; z zachowaniem zale?no?ci (dependantFor, Before, czy Triggers)
    public static int Main () => Execute<Build>(x => x.AngularRun);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    // Nuke udost?pnia klas? AbsolutePath, dzi?ki kt?rej mo?emy ?atwo odnosi? si? do r??nych lokalizacji
    // zalet? zapisywania ?cie?ek w takiej postaci (w por?wnaniu do zwyk?ego sztywnego stringa) jest fakt, ?e nuke sam zadba o znalezienie pierwszej
    // cz??ci ?cie?ki (kt?ra mo?e by? przecie? r??na na komputerach z macOS, Windowsem czy linuxem)
    // RootDirectory to miejsce, kt?re wskazali?my podczas komendy 'nuke :setup'
    // taki ?mieszny zapis z foreslash'ami nuke interpretuje jako Path.Combine -> czyli dok?ada dalszy ci?g ?cie?ki
    AbsolutePath SourceDirectory => RootDirectory / "api" / "lukaKry.Cal.API";
    AbsolutePath TestsDirectory => RootDirectory / "api" / "lukaKry.Calc.API.UnitTests";

    Target Clean => _ => _
        .DependentFor(Restore)
        .Executes(() =>
        {
            // dwie opcje znalaz?em 
            // pierwsza wykorzystuje zwyk?? komend? 'dotnet clean'
            // druga wykorzystuje zwyk?e usuni?cie zawarto?ci folder?w bin i obj; no w?a?nie - zawarto?ci. Na koniec pozostaj? puste foldery

            // metoda 1
            DotNetClean(s => s.SetProject(Solution.Directory));
            
            // metoda 2
            // SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            // TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);

            // jakie? r??nice? Ano takie, ?e dotnet clean czy?ci omawiane foldery z plik?w, o kt?rych VisualStudio ma jakie? info
            // je?li do tych folder?w sami przeniesiemy dowolny inny plik np. test.txt, to plik ten pozostanie nietkni?ty
            // natomiast usuwanie folder?w na twardo (metoda 2) oczywi?cie usunie ten plik
            // druga r??nica jest taka, ?e w pierwszym przypadku podajemy plik solucji, kt?ry b?dzie wyczyszczony;
            // w drugim przypadku sami wskazujemy na foldery, kt?re chcemy poczy?ci?
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            // przy metodzie dotnet restore nie ma za wiele do ustawiania konfiguracji
            // mo?na na przyk?ad wybra? plik  nuget.config, z kt?rego s? pobierane feeds dla r??nych paczek
            // jesli z jakiego? powodu chcemy wskaza? inny plik ?r?d?owy, to wtedy mo?na to ustawi? w tym targecie
            // jedynie warto zapami?ta?, ?e restore jest wbudowane w inne komendy dotnet jak np. run, build, test, publish
            // je?li teraz jawnie u?yjemy restore, to potem trzeba pami?ta?, aby u?y? flagi --no-restore, celem pomini?cia powt?rzenia tego kroku
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });


    // cel: skompilowa? aplikacj? i odpali? wybrane testy korzystaj?c z filtra dotnet test --filter
    // cel2: skompilowa? aplikacj? w kontenerze, opublikowa? j? do nast?pnego kontenera i odpali? testy
    // cel3: odpali? wszystkie projekty w kontenerach i puszczenie test?w

    Target UnitTestApi => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(RootDirectory / "api/lukaKry.Calc.API.UnitTests")
                .EnableNoRestore()
                .EnableNoBuild()
                .SetFilter("Category=urgent")
            );
        });


    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() => 
        {
            DotNetPublish(s => s
                .AddAuthors("lukaKry")
                .SetProject(Solution)
                .EnableNoBuild()
                .EnableNoRestore()
            );
        });


    Target CreateCommonNetwork => _ => _
        .DependsOn(UnitTestApi)
        .Executes(() =>
        {
            DockerNetworkCreate(s => s
                .SetDriver("bridge")
                .SetNetwork("multi")
            );
        });



    Target DbImage => _ => _
        .Executes(() =>
        {
            DockerBuild(s => s
               .SetPath(RootDirectory / "api")
               .SetFile(RootDirectory / "api" / "Dockerfile.mssql")
               .SetTag("db")
                );
        });

    Target DbRun => _ => _
        .DependsOn(DbImage, CreateCommonNetwork)
        .Executes(() =>
        {
            DockerRun(s => s
               .SetImage("db")
               .SetName("db")
               .SetHostname("db")
               .SetPublish("51001:1433")
               .SetDetach(true)
               .SetNetwork("multi")
                );
        });

    Target ApiImage => _ => _
        .Executes(() => 
        {
            DockerBuild( s => s
                .SetPath(RootDirectory / "api" )
                .SetFile(RootDirectory / "api" / "Dockerfile.api")
                .SetTag("api")
                );

            
        });

    Target ApiRun => _ => _
        .DependsOn(ApiImage, DbRun)
        .Executes(() => 
        {
            // teraz pozosta?y problemy z certyfikatem ssl
            DockerRun(s => s
                .SetImage("api") 
                .SetName("api")
                .SetPublish("51444:443")
                .SetPublish("52001:80")
                .SetDetach(true)
                .SetNetwork("multi")
                .SetProcessEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development")
                .SetProcessEnvironmentVariable("ASPNETCORE_URLS", "https://+:443;http://+:80")

            );
        });

    Target AngularImage => _ => _
        .Executes(() =>
        {
            DockerBuild(s => s
               .SetPath(RootDirectory / "angular")
               .SetFile(RootDirectory / "angular" / "Dockerfile.angular")
               .SetTag("angular")
                );
        });

    Target AngularRun => _ => _
        .DependsOn(AngularImage, ApiRun)
        .Executes(() =>
        {
            DockerRun(s => s
                .SetImage("angular")
                .SetHostname("localhost")
                .SetName("angular")
                .SetPublish("80:80")
                .SetDetach(true)
                .SetNetwork("multi")
            );
        });




    Target CleanUp => _ => _
        .Executes(() => 
        {
            DockerStop(s => s.SetContainers("api", "db", "angular"));
            DockerRm(s => s.SetContainers("api", "db", "angular"));
            DockerNetworkRm(s => s.SetNetworks("multi"));
        });

    
    
}
