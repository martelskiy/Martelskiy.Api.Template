# Martelskiy.Api.Template

## How to install the template

Assuming that you are standing in c:\temp

1. ``git clone https://github.com/martelskiy/Martelskiy.Api.Template.git``
2. Install the template ``dotnet new --install martelskiy.api.template\src``

## How to use the template

``dotnet new martelskiy.api -n MyProjectName -o MyOutputFolder``

## Good to know

### Locked dotnet core version - ``global.json``

## Template frameworks:

1. [``Serilog``](https://serilog.net/)
2. [``Swagger``](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
3. [``Cake build``](https://cakebuild.net/)

## Configuration description:

1. Custom error handling middleware
2. Health checks
3. Correlation ID configuration
4. Environment set up
5. Log into Elastic (default host address ``localhost:9200``)
6. Encapsulated Newtonsoft serializer
7. Basic Docker file added
8. Basic Cake build configuration