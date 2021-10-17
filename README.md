# Avalonia XAML Behaviors

[![Gitter](https://badges.gitter.im/wieslawsoltes/AvaloniaBehaviors.svg)](https://gitter.im/wieslawsoltes/AvaloniaBehaviors?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

[![Build Status](https://dev.azure.com/wieslawsoltes/GitHub/_apis/build/status/wieslawsoltes.AvaloniaBehaviors?repoName=wieslawsoltes%2FAvaloniaBehaviors&branchName=master)](https://dev.azure.com/wieslawsoltes/GitHub/_build/latest?definitionId=90&repoName=wieslawsoltes%2FAvaloniaBehaviors&branchName=master)
[![CI](https://github.com/wieslawsoltes/AvaloniaBehaviors/actions/workflows/build.yml/badge.svg)](https://github.com/wieslawsoltes/AvaloniaBehaviors/actions/workflows/build.yml)

[![NuGet](https://img.shields.io/nuget/v/Avalonia.Xaml.Behaviors.svg)](https://www.nuget.org/packages/Avalonia.Xaml.Behaviors)
[![NuGet](https://img.shields.io/nuget/dt/Avalonia.Xaml.Interactivity.svg)](https://www.nuget.org/packages/Avalonia.Xaml.Interactivity)
[![MyGet](https://img.shields.io/myget/xamlbehaviors-nightly/vpre/Avalonia.Xaml.Behaviors.svg?label=myget)](https://www.myget.org/gallery/xamlbehaviors-nightly) 

**AvaloniaBehaviors** is a port of [Windows UWP](https://github.com/Microsoft/XamlBehaviors) version of XAML Behaviors for [Avalonia](https://github.com/AvaloniaUI/Avalonia) XAML.

Avalonia XAML Behaviors is an easy-to-use means of adding common and reusable interactivity to your [Avalonia](https://github.com/AvaloniaUI/Avalonia) applications with minimal code. Avalonia port is available only for managed applications. Use of XAML Behaviors is governed by the MIT License. 

<a href='https://www.youtube.com/watch?v=pffBS-yQ_uM' target='_blank'>![](https://i.ytimg.com/vi/pffBS-yQ_uM/hqdefault.jpg)<a/>

## Building Avalonia XAML Behaviors

First, clone the repository or download the latest zip.
```
git clone https://github.com/wieslawsoltes/AvaloniaBehaviors.git
```

### Build on Windows using script

* [.NET Core](https://www.microsoft.com/net/download?initial-os=windows).

Open up a command-prompt and execute the commands:
```
.\build.ps1
```

### Build on Linux using script

* [.NET Core](https://www.microsoft.com/net/download?initial-os=linux).

Open up a terminal prompt and execute the commands:
```
./build.sh
```

### Build on OSX using script

* [.NET Core](https://www.microsoft.com/net/download?initial-os=macos).

Open up a terminal prompt and execute the commands:
```
./build.sh
```

## NuGet

Avalonia XamlBehaviors is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/Avalonia.Xaml.Behaviors/) and install the package like this:

`Install-Package Avalonia.Xaml.Behaviors`

or by using nightly build feed:
* Add `https://www.myget.org/F/xamlbehaviors-nightly/api/v2` to your package sources
* Alternative nightly build feed `https://pkgs.dev.azure.com/wieslawsoltes/GitHub/_packaging/Nightly/nuget/v3/index.json`
* Update your package using `XamlBehaviors` feed

and install the package like this:

`Install-Package Avalonia.Xaml.Behaviors -Pre`

### Package Sources

* https://api.nuget.org/v3/index.json
* https://www.myget.org/F/avalonia-ci/api/v2

## Resources

* [GitHub source code repository.](https://github.com/wieslawsoltes/AvaloniaBehaviors)

## License

Avalonia XAML Behaviors is licensed under the [MIT license](LICENSE.TXT).
