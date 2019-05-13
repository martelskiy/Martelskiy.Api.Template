#addin nuget:?package=Cake.Docker&version=0.10.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "BuildAndTest");
var configuration = Argument("configuration", "Release");
var dockerTag = Argument("dockertag", "latest");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var projectName = "Martelskiy.Api.Template";
var solutionFile = $"{projectName}.sln";
var dockerImageName = $"{projectName.ToLower()}:{dockerTag}";
var containerRegistryPath = $"";


// Define directories.
var buildDir = Directory($"../src/{projectName}/bin") + Directory(configuration);
var projectDir = Directory($"../src/{projectName}");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    Information($"Cleaning directory '{buildDir}'");
    CleanDirectory(buildDir);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() => {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration
        };
        DotNetCoreBuild($"../{solutionFile}", settings);
});

Task("Test")
    .Does(() => {
        var projectFiles = GetFiles("../test/**/*.csproj");
        var testSettings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true
        };
        foreach(var projectFile in projectFiles)
        {
            DotNetCoreTest(projectFile.FullPath, testSettings);
        }
});
Task("Publish")
    .Does(() => {
        var outputPath = "./artifacts/";
        Information($"Cleaning directory '{outputPath}'");
        CleanDirectory(outputPath);

        var settings = new DotNetCorePublishSettings
        {
            Configuration = configuration,
            OutputDirectory = outputPath,
            NoRestore = true
        };

        DotNetCorePublish($"../src/{projectName}/{projectName}.csproj", settings);
});

Task("DockerBuild")
    .Does(() => {
        var settings = new DockerImageBuildSettings 
        { 
            Tag = new[] { dockerImageName }
        };

        Information($"Building docker image {dockerImageName}");
        DockerBuild(settings, projectDir);
});

Task("DockerPush")
    .IsDependentOn("DockerBuild")
    .Does(() => {
            Information($"Pushing docker image {dockerImageName} to registry");
            DockerTag(dockerImageName, containerRegistryPath);
            DockerPush(containerRegistryPath);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("BuildAndTest")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
