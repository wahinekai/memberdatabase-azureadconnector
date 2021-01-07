# Member Database Azure AD Connector Repository

This repository holds code for the Azure AD Connector of the Wahine Kai Membership Database application.

## Building and running code

### Prerequisites

There are a few prerequisites required to develop this backend. These prerequisites are below.
Please download and install the prerequisites below to work with the code.

1.  [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0) is required to build and test the code.
2.  [Docker Desktop](https://www.docker.com/products/docker-desktop) is required to build, push, and otherwise work with the production Docker containers.
3.  [Python 3](https://www.python.org/downloads/) is required to run the build helper script (`build.py`).

### Development

To build a development version, run the following command from the root of this repository: `./build.py -d`.
This will build a development version of the application for testing. The application has hot reloading - meaning
any changes will be re-compiled without user intervention.

### Release

To test a release candidate locally, build the release Docker container with `./build.py -b`.
It is possible to then test that release version with `./build.py -p`.

## TO-DO

1.  Create documentation & build script for pushing to Github Packages.
2.  Create & integrate Docker Compose for development & release full-stack testing.
3.  Add local Cosmos DB emulator to script.
4.  Documentation on environment variable adding
