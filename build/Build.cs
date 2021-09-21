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

    // Znale�� co znacz� poni�sze dwie linijki
    AbsolutePath SourceDirectory => RootDirectory / "api" / "lukaKry.Cal.API";
    AbsolutePath TestsDirectory => RootDirectory / "api" / "lukaKry.Calc.API.UnitTests";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            // dwie opcje znalaz�em 
            // pierwsza wykorzystuje zwyk�� komend� 'dotnet clean'
            // druga wykorzystuje zwyk�e usuni�cie zawarto�ci folder�w bin i obj; no w�a�nie - zawarto�ci. Na koniec pozostaj� puste foldery

            DotNetClean(s => s.SetProject(Solution.Directory));
            // lub
            // SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            // TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);

            // jakie� r�nice? Ano takie, �e dotnet clean czy�ci omawiane foldery z plik�w, o kt�rych VisualStudio ma jakie� info
            // je�li do tych folder�w sami przeniesiemy dowolny inny plik np. test.txt, to plik ten pozostanie nietkni�ty
            // natomiast usuwanie folder�w na twardo oczywi�cie te� usunie ten plik
            // druga r�nica jest taka, �e w pierwszym przypadku podajemy plik solucji, kt�ry b�dzie wyczyszczony;
            // w drugim przypadku sami wskazujemy na foldery, kt�re chcemy poczy�ci�
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            // nast�pnego dnia zg��biamy tajniki polecenia dotnet restore - powinno p�j�� troch� szybciej

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
