## Install .net (SDK and runtime):

```bash
# choose latest
$ sudo dnf install dotnet-sdk-5.0
```

Video is [here](https://www.youtube.com/watch?v=Ar20aMQxR7I)

Here is an examples:

```bash
# simple project
dotnet new console -f net5.0 -lang f# -o SomeName
```
Your `.fsproj` should look something like:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
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

```plain
$ cd TestDemo.Tests/
$ dotnet test
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
