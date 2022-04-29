#!/bin/bash

set -xeo pipefail

dotnet clean
dotnet build --configuration Release
dotnet test
dotnet pack --configuration Release
file="$(ls -1 ./src/FluentEmail.Handlebars/bin/Release/FluentEmail.Handlebars.*.nupkg | sort -nr | head -n1)"
dotnet nuget push "$file" --source github
