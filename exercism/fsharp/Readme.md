# Manage .net projects with ```dotnet cli``` and ```Visual Code```

Install .net (SDK and runtime):

NOTE: ```preview``` packages should be excluded

```bash
$ sudo sh -c 'echo -e "[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl=https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc\nexclude=*preview*" > /etc/yum.repos.d/dotnetdev.repo'

# choose latest
$ sudo yum install dotnet-runtime-2.0.6.x86_64
$ sudo yum install dotnet-sdk-2.1.4.x86_6
```

Video is [here](https://www.youtube.com/watch?v=Ar20aMQxR7I)

Here is an examples:

```bash
# simple project
dotnet new console -f netcoreapp2.0 -lang f# -o SomeName
```
Your `.fsproj` should look something like:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <None Include="README.md" />
    <Compile Include="First.fs" />
    <Compile Include="Second.fs" />
    <Compile Include="Program.fs" />
    <None Include="Notes.md" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.7.2" />
    <PackageReference Include="xunit" Version="2.3.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.3.1" />
    <PackageReference Include="FsUnit.xUnit" Version="3.1.0" />
    <DotNetCliToolReference Include="dotnet-xunit" Version="2.3.1" />
  </ItemGroup>
</Project>
```

```bash

$ mkdir StudyFSharp && cd StudyFSharp

$ dotnet new classlib -lang f# -o TestDemo.Core
$ dotnet new xunit -lang f# -o TestDemo.Tests
$ dotnet add TestDemo.Tests/TestDemo.Tests.fsproj reference TestDemo.Core/TestDemo.Core.fsproj

$ cd TestDemo.Core/
$ dotnet restore build
$ dotnet restore
$ dotnet build

$ cd ../TestDemo.Tests/
$ dotnet restore
$ dotnet build

$ cd ..
$ dotnet new sln
$ dotnet sln StudyFSharp.sln add TestDemo.Core/TestDemo.Core.fsproj
$ dotnet sln StudyFSharp.sln add TestDemo.Tests/TestDemo.Tests.fsproj
$ dotnet build StudyFSharp.sln
```

```bash
$ cd TestDemo.Tests/
$ dotnet test

# or ctrl+shift+r from Visual Code
```

## Add dependencies

If you need project specific library search in http://nuget.org

```plain
$ cd <dotnet project>
$ dotnet add package FsUnit --version 3.1.0
```

## Add dependency with paket

See how paket should be configured ```dotnet cli``` [here](https://fsprojects.github.io/Paket/paket-and-dotnet-cli.html).

```plain
$ mono .paket/paket.exe init
$ mono .paket/paket.exe add FsUnit --version 3.1.0

$ mono .paket/paket.exe add xunit --version 2.4.0-beta.1.build3958
$ mono .paket/paket.exe add xunit.runner.console --version 2.4.0-beta.1.build3958
$ mono .paket/paket.exe add Microsoft.NETCore.Portable.Compatibility --version 1.0.1
```

Video is [here](https://www.youtube.com/watch?v=6ga1nu0BgCs)

## How to install Exercism

```plain

$ tar -xzvf exercism-linux-64bit.tgz
$ rm exercism-linux-64bit.tgz
$ mv exercism ~/bin

$ exercism configure --key=
$ mkdir -p ~/.config/exercism/
$ curl http://cli.exercism.io/shell/exercism\_completion.bash > ~/.config/exercism/exercism\_completion.bash
 emacs ~/.config/exercism/exercism_completion.bash
```

```bash
if [ -f ~/.config/exercism/exercism_completion.bash ]; then
  . ~/.config/exercism/exercism_completion.bash
fi
```

## How to work with Exercism

```plain
$ exercism restore
$ exercism fetch

Go to the new directory and read Readme.md

$ exercism submit <path to the F# file>
$ exercism fetch
```

## How to update Exercism client

```plain
$ exercsism upgade
```

## VS Code

Install the `Ionide` package for F# development
```
code --install-extension  Ionide.Ionide-fsharp
```

Finally we need to configure `Ionide` to use `dotnet core` rather than `mono`.
Open up `VS Code` and navigate to `File -> Preferences`. Search for setting
`FSharp.fsacRuntime` by default it has value _"net"_ which corresponds to `mono`.
Change this value to _"netcore"_ and **restart** VS Code.
