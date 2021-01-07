#!/usr/bin/env python3

from optparse import OptionParser
from os import system, getenv

# Globals
sln = "./src/AzureAdConnector.sln"
dockerContainerName = "wahinekai/memberdatabase-azureadconnector"

# For dotnet watch run, needs to be relative to sln
project = "./AzureAdConnectorHost/AzureAdConnectorHost.csproj"

def parse_command_line_arguments():
    parser = OptionParser()
    parser.add_option("-c", "--clean", action="store_true", dest="clean", default=False, help="Clean the project")
    parser.add_option("-r", "--restore", action="store_true", dest="restore", default=False, help="Restore project dependencies")
    parser.add_option("-b", "--build", action="store_true", dest="build", default=False, help="Build Project")
    parser.add_option("-t", "--test", action="store_true", dest="test", default=False, help="Test Project")
    parser.add_option("-d", "--run-development", action="store_true", dest="run_development", default=False, help="Run dotnet watch run to build/run a hot reloading development version of the backend")
    parser.add_option("-B", "--build-docker-container", action="store_true", dest="build_docker", default=False, help="Build production Docker Container")
    parser.add_option("-P", "--run-production", action="store_true", dest="run_production", default=False, help="Run Production Docker Container")
    parser.add_option("-o", "--output", dest="output", default="./out", help="Project output directory, defaults to ./out")
    parser.add_option("-C", "--configuration", dest="configuration", default="Debug", help="Project configuration type, defaults to Debug")
    return parser.parse_args()

def dotnetRestore():
    print("Restoring Project Dependencies")
    system(f"dotnet restore {sln}")

def dotnetBuild(configuration, output):
    print("Building project")
    system(f"dotnet build {sln} --no-restore --configuration {configuration} -o {output}")

def dotnetClean(output):
    print("Cleaning project")
    system(f"rm -rf {output}")
    system(f"dotnet clean {sln}")

def dotnetTest(configuration, output):
    print("Testing project")
    system(f"dotnet test {sln} --no-build --configuration {configuration} /p:OutputPath={output}")

def dockerBuild():
    print("Building Docker container")
    packages_token = getenv('GITHUB_PACKAGES_TOKEN')
    system(f"docker build . -t {dockerContainerName} --build-arg ASPNETCORE_ENVIRONMENT=Development --build-arg PACKAGES_TOKEN={packages_token}")

def dockerRun():
    system(f"docker run --rm -it -p 80:80 {dockerContainerName}")

def dotnetWatchRun(configuration):
    print("Running development version")
    system(f"dotnet watch --project {sln} run --project {project} --configuration {configuration}")

def main():
    print("Parsing command-line arguments")
    (options, _args) = parse_command_line_arguments()

    if options.clean:
        dotnetClean(options.output)

    if options.restore:
        dotnetRestore()

    if options.build:
        dotnetBuild(options.configuration, options.output)

    if options.test:
        dotnetTest(options.configuration, options.output)

    if options.run_development:
        dotnetWatchRun(options.configuration)

    if options.build_docker:
        dockerBuild()

    if options.run_production:
        dockerRun()

if __name__ == "__main__":
    main()