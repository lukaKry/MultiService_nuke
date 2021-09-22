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
    // jeœli wpiszemy x.Compile, to pierwszy odpali siê target o nazwie Compile; z zachowaniem zale¿noœci (dependantFor, Before, czy Triggers)
    public static int Main () => Execute<Build>(x => x.dbRun);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    // Nuke udostêpnia klasê AbsolutePath, dziêki której mo¿emy ³atwo odnosiæ siê do ró¿nych lokalizacji
    // zalet¹ zapisywania œcie¿ek w takiej postaci (w porównaniu do zwyk³ego sztywnego stringa) jest fakt, ¿e nuke sam zadba o znalezienie pierwszej
    // czêœci œcie¿ki (która mo¿e byæ przecie¿ ró¿na na komputerach z macOS, Windowsem czy linuxem)
    // RootDirectory to miejsce, które wskazaliœmy podczas komendy 'nuke :setup'
    // taki œmieszny zapis z foreslash'ami nuke interpretuje jako Path.Combine -> czyli dok³ada dalszy ci¹g œcie¿ki
    AbsolutePath SourceDirectory => RootDirectory / "api" / "lukaKry.Cal.API";
    AbsolutePath TestsDirectory => RootDirectory / "api" / "lukaKry.Calc.API.UnitTests";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            // dwie opcje znalaz³em 
            // pierwsza wykorzystuje zwyk³¹ komendê 'dotnet clean'
            // druga wykorzystuje zwyk³e usuniêcie zawartoœci folderów bin i obj; no w³aœnie - zawartoœci. Na koniec pozostaj¹ puste foldery

            // metoda 1
            DotNetClean(s => s.SetProject(Solution.Directory));
            
            // metoda 2
            // SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            // TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);

            // jakieœ ró¿nice? Ano takie, ¿e dotnet clean czyœci omawiane foldery z plików, o których VisualStudio ma jakieœ info
            // jeœli do tych folderów sami przeniesiemy dowolny inny plik np. test.txt, to plik ten pozostanie nietkniêty
            // natomiast usuwanie folderów na twardo (metoda 2) oczywiœcie usunie ten plik
            // druga ró¿nica jest taka, ¿e w pierwszym przypadku podajemy plik solucji, który bêdzie wyczyszczony;
            // w drugim przypadku sami wskazujemy na foldery, które chcemy poczyœciæ
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            // przy metodzie dotnet restore nie ma za wiele do ustawiania konfiguracji
            // mo¿na na przyk³ad wybraæ plik  nuget.config, z którego s¹ pobierane feeds dla ró¿nych paczek
            // jesli z jakiegoœ powodu chcemy wskazaæ inny plik Ÿród³owy, to wtedy mo¿na to ustawiæ w tym targecie
            // jedynie warto zapamiêtaæ, ¿e restore jest wbudowane w inne komendy dotnet jak np. run, build, test, publish
            // jeœli teraz jawnie u¿yjemy restore, to potem trzeba pamiêtaæ, aby u¿yæ flagi --no-restore, celem pominiêcia powtórzenia tego kroku

            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            // teraz obczajamy komendê dotnet build

            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });


    // cel: skompilowaæ aplikacjê i odpaliæ wybrane testy korzystaj¹c z filtra dotnet test --filter
    // cel2: skompilowaæ aplikacjê w kontenerze, opublikowaæ j¹ do nastêpnego kontenera i odpaliæ testy
    // cel3: odpaliæ wszystkie projekty w kontenerach i puszczenie testów

    Target TestApi => _ => _
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

    Target apiImage => _ => _
        .Executes(() => 
        {
            // stworzenie obrazu dla kontenera z api

            DockerBuild( s => s
                .SetPath(RootDirectory / "api" / "lukaKry.Cal.api")
                .SetFile(SourceDirectory / "Dockerfile.api")
                .SetTag("api")
                );

            
        });

    Target apiRun => _ => _
        .Executes(() => 
        { 
            
        });

    Target dbRun => _ => _
        .Executes(() => 
        {
            // pytanie nr 1 - jak dodaæ zmienne œrodowiskowe, bom ja tego nie widzim

            DockerRun(s => s
               .AddProcessEnvironmentVariable("ACCEPT_EULA", "Y")
               .AddProcessEnvironmentVariable("SA_PASSWORD", "P@ssword123")
               .SetImage("mcr.microsoft.com/mssql/server:2019-latest")
               .SetName("db")
               .SetHostname("db")
               .SetPublish("51433:51433")
               .SetDetach(true)
                );
        });
}
