# Avalonia XAML Behaviors

[![Gitter](https://badges.gitter.im/wieslawsoltes/AvaloniaBehaviors.svg)](https://gitter.im/wieslawsoltes/AvaloniaBehaviors?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

[![Build status](https://dev.azure.com/wieslawsoltes/GitHub/_apis/build/status/Sources/AvaloniaBehaviors)](https://dev.azure.com/wieslawsoltes/GitHub/_build/latest?definitionId=53)

[![NuGet](https://img.shields.io/nuget/v/Avalonia.Xaml.Behaviors.svg)](https://www.nuget.org/packages/Avalonia.Xaml.Behaviors) [![MyGet](https://img.shields.io/myget/xamlbehaviors-nightly/vpre/Avalonia.Xaml.Behaviors.svg?label=myget)](https://www.myget.org/gallery/xamlbehaviors-nightly) 

**AvaloniaBehaviors** is a port of [Windows UWP](https://github.com/Microsoft/XamlBehaviors) version of XAML Behaviors for [Avalonia](https://github.com/AvaloniaUI/Avalonia) XAML.

Avalonia XAML Behaviors is an easy-to-use means of adding common and reusable interactivity to your [Avalonia](https://github.com/AvaloniaUI/Avalonia) applications with minimal code. Avalonia port is available only for managed applications. Use of XAML Behaviors is governed by the MIT License. 

<a href='https://www.youtube.com/watch?v=pffBS-yQ_uM' target='_blank'>![](https://i.ytimg.com/vi/pffBS-yQ_uM/hqdefault.jpg)<a/>

## Example Usage

Example of using Behaviors in an `Avalonia`  XAML application:

```XAML
<Window xmlns="https://github.com/avaloniaui"
        xmlns:i="clr-namespace:Avalonia.Xaml.Interactivity;assembly=Avalonia.Xaml.Interactivity"
        xmlns:ia="clr-namespace:Avalonia.Xaml.Interactions.Core;assembly=Avalonia.Xaml.Interactions"
        Width="500" Height="400">
    <Grid RowDefinitions="Auto,100">
        <TextBox Name="textBox" Text="Hello" Grid.Row="0" Margin="5"/>
        <Button Name="changePropertyButton" Content="Change Property" Grid.Row="1" Margin="5,0,5,5">
            <i:Interaction.Behaviors>
                <ia:EventTriggerBehavior EventName="Click" SourceObject="{Binding #changePropertyButton}">
                    <ia:ChangePropertyAction TargetObject="{Binding #textBox}" PropertyName="Text" Value="World"/>
                </ia:EventTriggerBehavior>
            </i:Interaction.Behaviors>
        </Button>
    </Grid>
</Window>
```

More examples can be found in [sample application](https://github.com/wieslawsoltes/AvaloniaBehaviors/tree/master/samples/BehaviorsTestApplication/Controls).

## Building Avalonia XAML Behaviors

First, clone the repository or download the latest zip.
```
git clone https://github.com/wieslawsoltes/AvaloniaBehaviors.git
git submodule update --init --recursive
```

### Build using IDE

* [Visual Studio Community 2017](https://www.visualstudio.com/pl/vs/community/) for `Windows` builds.
* [MonoDevelop](http://www.monodevelop.com/) for `Linux` builds.

Open `AvaloniaBehaviors.sln` in selected IDE and run `Build` command.

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
* Alternative nightly build feed `https://pkgs.dev.azure.com/wieslawsoltes/GitHub/_packaging/CI/nuget/v3/index.json`
* Update your package using `XamlBehaviors` feed

and install the package like this:

`Install-Package Avalonia.Xaml.Behaviors -Pre`

### NuGet Packages

* [Avalonia.Xaml.Interactivity](https://www.nuget.org/packages/Avalonia.Xaml.Interactivity/) - Core library.
* [Avalonia.Xaml.Interactions](https://www.nuget.org/packages/Avalonia.Xaml.Interactions/) - Default actions and behaviors.
* [Avalonia.Xaml.Interactions.Custom](https://www.nuget.org/packages/Avalonia.Xaml.Interactions.Custom/) - Custom actions and behaviors.
* [Avalonia.Xaml.Behaviors](https://www.nuget.org/packages/Avalonia.Xaml.Behaviors/) - Meta package containing core library and default actions and behaviors

### Package Dependencies

* [Avalonia](https://www.nuget.org/packages/Avalonia/)
* [System.Reactive](https://www.nuget.org/packages/System.Reactive/)
* [System.Reactive.Core](https://www.nuget.org/packages/System.Reactive.Core/)
* [System.Reactive.Interfaces](https://www.nuget.org/packages/System.Reactive.Interfaces/)
* [System.Reactive.Linq](https://www.nuget.org/packages/System.Reactive.Linq/)
* [System.Reactive.PlatformServices](https://www.nuget.org/packages/System.Reactive.PlatformServices/)
* [Serilog](https://www.nuget.org/packages/Serilog/)
* [Splat](https://www.nuget.org/packages/Splat/)
* [Sprache](https://www.nuget.org/packages/Sprache/)

### Package Sources

* https://api.nuget.org/v3/index.json
* https://www.myget.org/F/avalonia-ci/api/v2

## Resources

* [GitHub source code repository.](https://github.com/wieslawsoltes/AvaloniaBehaviors)

## License

Avalonia XAML Behaviors is licensed under the [MIT license](LICENSE.TXT).
