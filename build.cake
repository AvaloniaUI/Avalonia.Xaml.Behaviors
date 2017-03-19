///////////////////////////////////////////////////////////////////////////////
// ADDINS
///////////////////////////////////////////////////////////////////////////////

#addin "nuget:?package=Polly&version=5.0.6"
#addin "nuget:?package=NuGet.Core&version=2.12.0"

///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////

#tool "nuget:?package=xunit.runner.console&version=2.2.0"
#tool "nuget:https://dotnet.myget.org/F/nuget-build/?package=NuGet.CommandLine&version=4.3.0-beta1-2361&prerelease"

///////////////////////////////////////////////////////////////////////////////
// USINGS
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Polly;
using NuGet;

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var platform = Argument("platform", "Any CPU");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// CONFIGURATION
///////////////////////////////////////////////////////////////////////////////

var MainRepo = "wieslawsoltes/AvaloniaBehaviors";
var MasterBranch = "master";
var AssemblyInfoPath = File("./src/Shared/SharedAssemblyInfo.cs");
var ReleasePlatform = "Any CPU";
var ReleaseConfiguration = "Release";
var MSBuildSolution = "./AvaloniaBehaviors.sln";
var XBuildSolution = "./AvaloniaBehaviors.sln";
var UnitTestsFramework = "net461";

///////////////////////////////////////////////////////////////////////////////
// PARAMETERS
///////////////////////////////////////////////////////////////////////////////

var isPlatformAnyCPU = StringComparer.OrdinalIgnoreCase.Equals(platform, "Any CPU");
var isPlatformX86 = StringComparer.OrdinalIgnoreCase.Equals(platform, "x86");
var isPlatformX64 = StringComparer.OrdinalIgnoreCase.Equals(platform, "x64");
var isLocalBuild = BuildSystem.IsLocalBuild;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var isRunningOnAppVeyor = BuildSystem.AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = BuildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
var isMainRepo = StringComparer.OrdinalIgnoreCase.Equals(MainRepo, BuildSystem.AppVeyor.Environment.Repository.Name);
var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals(MasterBranch, BuildSystem.AppVeyor.Environment.Repository.Branch);
var isTagged = BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag 
               && !string.IsNullOrWhiteSpace(BuildSystem.AppVeyor.Environment.Repository.Tag.Name);
var isReleasable = StringComparer.OrdinalIgnoreCase.Equals(ReleasePlatform, platform) 
                   && StringComparer.OrdinalIgnoreCase.Equals(ReleaseConfiguration, configuration);
var isMyGetRelease = !isTagged && isReleasable;
var isNuGetRelease = isTagged && isReleasable;

///////////////////////////////////////////////////////////////////////////////
// VERSION
///////////////////////////////////////////////////////////////////////////////

var version = ParseAssemblyInfo(AssemblyInfoPath).AssemblyVersion;

if (isRunningOnAppVeyor)
{
    if (isTagged)
    {
        // Use Tag Name as version
        version = BuildSystem.AppVeyor.Environment.Repository.Tag.Name;
    }
    else
    {
        // Use AssemblyVersion with Build as version
        version += "-build" + EnvironmentVariable("APPVEYOR_BUILD_NUMBER") + "-alpha";
    }
}

///////////////////////////////////////////////////////////////////////////////
// DIRECTORIES
///////////////////////////////////////////////////////////////////////////////

var artifactsDir = (DirectoryPath)Directory("./artifacts");
var testResultsDir = artifactsDir.Combine("test-results");
var nugetRoot = artifactsDir.Combine("nuget");
var dirSuffix = isPlatformAnyCPU ? configuration : platform + "/" + configuration;
var buildDirs = 
    GetDirectories("./src/**/bin/" + dirSuffix) + 
    GetDirectories("./src/**/obj/" + dirSuffix) + 
    GetDirectories("./samples/**/bin/" + dirSuffix) + 
    GetDirectories("./samples/**/obj/" + dirSuffix) +
    GetDirectories("./tests/**/bin/" + dirSuffix) + 
    GetDirectories("./tests/**/obj/" + dirSuffix);

///////////////////////////////////////////////////////////////////////////////
// NUGET NUSPECS
///////////////////////////////////////////////////////////////////////////////

// Key: Package Id
// Value is Tuple where Item1: Package Version, Item2: The *.csproj/*.props file path.
var packageVersions = new Dictionary<string, IList<Tuple<string,string>>>();

System.IO.Directory.EnumerateFiles(((DirectoryPath)Directory("./build")).FullPath, "*.props", SearchOption.AllDirectories)
    .ToList()
    .ForEach(fileName => {
    var xdoc = XDocument.Load(fileName);
    foreach (var reference in xdoc.Descendants().Where(x => x.Name.LocalName == "PackageReference"))
    {
        var name = reference.Attribute("Include").Value;
        var versionAttribute = reference.Attribute("Version");
        var packageVersion = versionAttribute != null 
            ? versionAttribute.Value 
            : reference.Elements().First(x=>x.Name.LocalName == "Version").Value;
        IList<Tuple<string, string>> versions;
        packageVersions.TryGetValue(name, out versions);
        if (versions == null)
        {
            versions = new List<Tuple<string, string>>();
            packageVersions[name] = versions;
        }
        versions.Add(Tuple.Create(packageVersion, fileName));
    }
});

Information("Checking installed NuGet package dependencies versions:");

packageVersions.ToList().ForEach(package =>
{
    var packageVersion = package.Value.First().Item1;
    bool isValidVersion = package.Value.All(x => x.Item1 == packageVersion);
    if (!isValidVersion)
    {
        Information("Error: package {0} has multiple versions installed:", package.Key);
        foreach (var v in package.Value)
        {
            Information("{0}, file: {1}", v.Item1, v.Item2);
        }
        throw new Exception("Detected multiple NuGet package version installed for different projects.");
    }
});

Information("Setting NuGet package dependencies versions:");

var AvaloniaVersion = packageVersions["Avalonia"].FirstOrDefault().Item1;

Information("Package: Avalonia, version: {0}", AvaloniaVersion);

var nuspecNuGetBehaviors = new NuGetPackSettings()
{
    Id = "Avalonia.Xaml.Behaviors",
    Version = version,
    Authors = new [] { "wieslaw.soltes" },
    Owners = new [] { "wieslaw.soltes" },
    LicenseUrl = new Uri("http://opensource.org/licenses/MIT"),
    ProjectUrl = new Uri("https://github.com/wieslawsoltes/AvaloniaBehaviors/"),
    RequireLicenseAcceptance = false,
    Symbols = false,
    NoPackageAnalysis = true,
    Description = "Easily add interactivity to your Avalonia apps using XAML Behaviors. Behaviors encapsulate reusable functionalities for elements that can be easily added to your XAML without the need for more imperative code.",
    Copyright = "Copyright 2016",
    Tags = new [] { "Avalonia", "Behavior", "Action", "Behaviors", "Actions", "Managed", "C#", "Interaction", "Interactivity", "Interactions", "Xaml" },
    Dependencies = new []
    {
        new NuSpecDependency { Id = "Avalonia", Version = AvaloniaVersion },
    },
    Files = new []
    {
        // netstandard1.1
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactivity/bin/" + dirSuffix + "/netstandard1.1/" + "Avalonia.Xaml.Interactivity.dll", Target = "lib/netstandard1.1" },
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactivity/bin/" + dirSuffix + "/netstandard1.1/" + "Avalonia.Xaml.Interactivity.xml", Target = "lib/netstandard1.1" },
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactions/bin/" + dirSuffix + "/netstandard1.1/" + "Avalonia.Xaml.Interactions.dll", Target = "lib/netstandard1.1" },
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactions/bin/" + dirSuffix + "/netstandard1.1/" + "Avalonia.Xaml.Interactions.xml", Target = "lib/netstandard1.1" },
        // net45
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactivity/bin/" + dirSuffix + "/net45/" + "Avalonia.Xaml.Interactivity.dll", Target = "lib/net45" },
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactivity/bin/" + dirSuffix + "/net45/" + "Avalonia.Xaml.Interactivity.xml", Target = "lib/net45" },
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactions/bin/" + dirSuffix + "/net45/" + "Avalonia.Xaml.Interactions.dll", Target = "lib/net45" },
        new NuSpecContent { Source = "src/Avalonia.Xaml.Interactions/bin/" + dirSuffix + "/net45/" + "Avalonia.Xaml.Interactions.xml", Target = "lib/net45" }
    },
    BasePath = Directory("./"),
    OutputDirectory = nugetRoot
};

var nuspecNuGetSettings = new List<NuGetPackSettings>();

nuspecNuGetSettings.Add(nuspecNuGetBehaviors);

var nugetPackages = nuspecNuGetSettings.Select(nuspec => {
    return nuspec.OutputDirectory.CombineWithFilePath(string.Concat(nuspec.Id, ".", nuspec.Version, ".nupkg"));
}).ToArray();

///////////////////////////////////////////////////////////////////////////////
// INFORMATION
///////////////////////////////////////////////////////////////////////////////

Information("Building version {0} of AvaloniaBehaviors ({1}, {2}, {3}) using version {4} of Cake.", 
    version,
    platform,
    configuration,
    target,
    typeof(ICakeContext).Assembly.GetName().Version.ToString());

if (isRunningOnAppVeyor)
{
    Information("Repository Name: " + BuildSystem.AppVeyor.Environment.Repository.Name);
    Information("Repository Branch: " + BuildSystem.AppVeyor.Environment.Repository.Branch);
}

Information("Target: " + target);
Information("Platform: " + platform);
Information("Configuration: " + configuration);
Information("IsLocalBuild: " + isLocalBuild);
Information("IsRunningOnUnix: " + isRunningOnUnix);
Information("IsRunningOnWindows: " + isRunningOnWindows);
Information("IsRunningOnAppVeyor: " + isRunningOnAppVeyor);
Information("IsPullRequest: " + isPullRequest);
Information("IsMainRepo: " + isMainRepo);
Information("IsMasterBranch: " + isMasterBranch);
Information("IsTagged: " + isTagged);
Information("IsReleasable: " + isReleasable);
Information("IsMyGetRelease: " + isMyGetRelease);
Information("IsNuGetRelease: " + isNuGetRelease);

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories(buildDirs);
    CleanDirectory(artifactsDir);
    CleanDirectory(testResultsDir);
    CleanDirectory(nugetRoot);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var maxRetryCount = 5;
    var toolTimeout = 1d;
    Policy
        .Handle<Exception>()
        .Retry(maxRetryCount, (exception, retryCount, context) => {
            if (retryCount == maxRetryCount)
            {
                throw exception;
            }
            else
            {
                Verbose("{0}", exception);
                toolTimeout+=0.5;
            }})
        .Execute(()=> {
            if(isRunningOnWindows)
            {
                NuGetRestore(MSBuildSolution, new NuGetRestoreSettings {
                    ToolPath = "./tools/NuGet.CommandLine/tools/NuGet.exe",
                    ToolTimeout = TimeSpan.FromMinutes(toolTimeout)
                });
            }
            else
            {
                NuGetRestore(XBuildSolution, new NuGetRestoreSettings {
                    ToolPath = "./tools/NuGet.CommandLine/tools/NuGet.exe",
                    ToolTimeout = TimeSpan.FromMinutes(toolTimeout)
                });
            }
        });
});

void DotNetCoreBuild()
{
    DotNetCoreRestore("./samples/BehaviorsTestApplication.NetCore");
    DotNetBuild("./samples/BehaviorsTestApplication.NetCore");
}

Task("DotNetCoreBuild")
    .IsDependentOn("Clean")
    .Does(() => 
{
    DotNetCoreBuild();
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if (isRunningOnWindows)
    {
        MSBuild(MSBuildSolution, settings => {
            settings.WithProperty("UseRoslynPathHack", "true");
            settings.UseToolVersion(MSBuildToolVersion.VS2017);
            settings.SetConfiguration(configuration);
            settings.WithProperty("Platform", "\"" + platform + "\"");
            settings.SetVerbosity(Verbosity.Minimal);
        });
    }
    else
    {
        DotNetCoreBuild();
    }
});

void RunCoreTest(string dir, bool isRunningOnWindows, bool net461Only)
{
    Information("Running tests from " + dir);
    DotNetCoreRestore(dir);
    var frameworks = new List<string>() { "netcoreapp1.1" };
    if (isRunningOnWindows)
        frameworks.Add("net461");
    foreach(var fw in frameworks)
    {
        if(fw != "net461" && net461Only)
            continue;
        Information("Running for " + fw);
        DotNetCoreTest(System.IO.Path.Combine(
            dir, 
            System.IO.Path.GetFileName(dir) + ".csproj"),
            new DotNetCoreTestSettings { Framework = fw });
    }
}

Task("Run-Net-Core-Unit-Tests")
    .IsDependentOn("Clean")
    .Does(() => 
{
    RunCoreTest("./tests/Avalonia.Xaml.Interactivity.UnitTests", isRunningOnWindows, false);
    RunCoreTest("./tests/Avalonia.Xaml.Interactions.UnitTests", isRunningOnWindows, false);
});

Task("Run-Unit-Tests")
    .IsDependentOn("Run-Net-Core-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    if(!isRunningOnWindows)
       return;
    var assemblies = GetFiles("./tests/**/bin/" + dirSuffix + "/" + UnitTestsFramework + "/*.UnitTests.dll");
    var settings = new XUnit2Settings { 
        ToolPath = (isPlatformAnyCPU || isPlatformX86) ? 
            "./tools/xunit.runner.console/tools/xunit.console.x86.exe" :
            "./tools/xunit.runner.console/tools/xunit.console.exe",
        OutputDirectory = testResultsDir,
        XmlReportV1 = true,
        NoAppDomain = true,
        Parallelism = ParallelismOption.None,
        ShadowCopy = false
    };
    foreach (var assembly in assemblies)
    {
        XUnit2(assembly.FullPath, settings);
    }
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    foreach(var nuspec in nuspecNuGetSettings)
    {
        NuGetPack(nuspec);
    }
});

Task("Publish-MyGet")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => !isLocalBuild)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isMainRepo)
    .WithCriteria(() => isMasterBranch)
    .WithCriteria(() => isMyGetRelease)
    .Does(() =>
{
    var apiKey = EnvironmentVariable("MYGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) 
    {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    var apiUrl = EnvironmentVariable("MYGET_API_URL");
    if(string.IsNullOrEmpty(apiUrl)) 
    {
        throw new InvalidOperationException("Could not resolve MyGet API url.");
    }

    foreach(var nupkg in nugetPackages)
    {
        NuGetPush(nupkg, new NuGetPushSettings {
            Source = apiUrl,
            ApiKey = apiKey
        });
    }
})
.OnError(exception =>
{
    Information("Publish-MyGet Task failed, but continuing with next Task...");
});

Task("Publish-NuGet")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => !isLocalBuild)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isMainRepo)
    .WithCriteria(() => isMasterBranch)
    .WithCriteria(() => isNuGetRelease)
    .Does(() =>
{
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) 
    {
        throw new InvalidOperationException("Could not resolve NuGet API key.");
    }

    var apiUrl = EnvironmentVariable("NUGET_API_URL");
    if(string.IsNullOrEmpty(apiUrl)) 
    {
        throw new InvalidOperationException("Could not resolve NuGet API url.");
    }

    foreach(var nupkg in nugetPackages)
    {
        NuGetPush(nupkg, new NuGetPushSettings {
            ApiKey = apiKey,
            Source = apiUrl
        });
    }
})
.OnError(exception =>
{
    Information("Publish-NuGet Task failed, but continuing with next Task...");
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Package")
  .IsDependentOn("Create-NuGet-Packages");

Task("Default")
    .Does(() =>
{
    if (isRunningOnWindows)
        RunTarget("Package");
    else
        RunTarget("Run-Net-Core-Unit-Tests");
});

Task("AppVeyor")
  .IsDependentOn("Publish-MyGet")
  .IsDependentOn("Publish-NuGet");

Task("Travis")
  .IsDependentOn("Run-Net-Core-Unit-Tests");

///////////////////////////////////////////////////////////////////////////////
// EXECUTE
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
