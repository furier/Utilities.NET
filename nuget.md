# Publishing package to nuget.org

- `cinst NuGet.CommandLine`
- `nuget pack YourPackage.nuspec`
- `nuget setApiKey <key>`
- `nuget push YourPackage.nupkg -Source https://www.nuget.org/api/v2/package`