# Manage .net projects with ```dotnet cli``` and ```Visual Code```

Install .net (SDK and runtime):

NOTE: ```preview``` packages should be excluded

```bash
$ sudo sh -c 'echo -e "[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl=https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc\nexclude=*preview*" > /etc/yum.repos.d/dotnetdev.repo'

# choose latest
$ sudo yum install dotnet-runtime-2.0.6.x86_64
$ sudo yum install dotnet-sdk-2.1.4.x86_6
```

Here is an example:

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

$ mkdir .vscode
$ emacs .vscode/tasks.json
```

This task calls out to .NET Core 2.0 CLI to run tests.
```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "Tests",
            "command": "dotnet",
            "type": "shell",
            "args": [
                "test"
            ],
            "presentation": {
                "reveal": "silent"
            },
            "problemMatcher": "$msCompile",
            "group": {
                "kind": "test",
                "isDefault": true
            }
        }
    ]
}
```

Add a keybinding in _keybindings.json_

```json
{   "key": "ctrl+shift+r",
    "command": "workbench.action.tasks.test"
}
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