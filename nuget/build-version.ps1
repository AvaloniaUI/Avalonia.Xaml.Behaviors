$ErrorActionPreference = "Stop"

. ".\include.ps1"

foreach($pkg in $Packages) 
{
    rm -Force -Recurse .\$pkg -ErrorAction SilentlyContinue
}

rm -Force -Recurse *.nupkg -ErrorAction SilentlyContinue
Copy-Item template Avalonia.Xaml.Behaviors -Recurse
sv lib "Avalonia.Xaml.Behaviors\lib\portable-windows8+net45"

mkdir $lib -ErrorAction SilentlyContinue

Copy-Item ..\src\Avalonia.Xaml.Interactivity\bin\Release\Avalonia.Xaml.Interactivity.dll $lib
Copy-Item ..\src\Avalonia.Xaml.Interactivity\bin\Release\Avalonia.Xaml.Interactivity.xml $lib
Copy-Item ..\src\Avalonia.Xaml.Interactions\bin\Release\Avalonia.Xaml.Interactions.dll $lib
Copy-Item ..\src\Avalonia.Xaml.Interactions\bin\Release\Avalonia.Xaml.Interactions.xml $lib

foreach($pkg in $Packages)
{
    (gc Avalonia.Xaml.Behaviors\$pkg.nuspec).replace('#VERSION#', $args[0]) | sc $pkg\$pkg.nuspec
}

foreach($pkg in $Packages)
{
    nuget.exe pack $pkg\$pkg.nuspec
}

foreach($pkg in $Packages)
{
    rm -Force -Recurse .\$pkg
}