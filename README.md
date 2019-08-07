# Pha3l.DotBump

DotBump is a utility for managing the version for a .NET Core project according to semver.  
It's useful for projects that are published to a NuGet feed.

## Installation
DotBump is a global cli tool, and can be installed as such:
```bash
$ dotnet tool install -g Pha3l.DotBump
```

## Usage
There's not much to it at the moment.  There are three actions supported.

### Init
Creates a new `.version` file at the current working directory with initial version `1.0.0`
```bash
$ dotbump init
```

### Bump
Bump the major version
```bash
$ cat .version # => 1.0.0
$ dotbump bump major
$ cat .version # => 2.0.0
```
`major` can be replaced with `minor` or `patch` to bump the corresponding semver component

### Miscellaneous
Once you have the `.version` file, it can be used with `dotnet pack` or other tools to specify the version.
I'm using it like this:
```bash
$ dotnet pack /p:Version=$(cat .version)
```
This, unlike having the version hardcoded in the `.csproj`, allows easily writing a shell script that will bump the patch version before pushing a new version to a NuGet feed.
