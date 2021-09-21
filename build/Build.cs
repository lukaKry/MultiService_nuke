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

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    // ZnaleŸæ co znacz¹ poni¿sze dwie linijki
    AbsolutePath SourceDirectory => RootDirectory / "api" / "lukaKry.Cal.API";
    AbsolutePath TestsDirectory => RootDirectory / "api" / "lukaKry.Calc.API.UnitTests";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            // dwie opcje znalaz³em 
            // pierwsza wykorzystuje zwyk³¹ komendê 'dotnet clean'
            // druga wykorzystuje zwyk³e usuniêcie zawartoœci folderów bin i obj; no w³aœnie - zawartoœci. Na koniec pozostaj¹ puste foldery

            DotNetClean(s => s.SetProject(Solution.Directory));
            // lub
            // SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            // TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);

            // jakieœ ró¿nice? Ano takie, ¿e dotnet clean czyœci omawiane foldery z plików, o których VisualStudio ma jakieœ info
            // jeœli do tych folderów sami przeniesiemy dowolny inny plik np. test.txt, to plik ten pozostanie nietkniêty
            // natomiast usuwanie folderów na twardo oczywiœcie te¿ usunie ten plik
            // druga ró¿nica jest taka, ¿e w pierwszym przypadku podajemy plik solucji, który bêdzie wyczyszczony;
            // w drugim przypadku sami wskazujemy na foldery, które chcemy poczyœciæ
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            // nastêpnego dnia zg³êbiamy tajniki polecenia dotnet restore - powinno pójœæ trochê szybciej

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

}
