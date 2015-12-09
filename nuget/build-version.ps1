$ErrorActionPreference = "Stop"

. ".\include.ps1"

foreach($pkg in $Packages) 
{
    rm -Force -Recurse .\$pkg -ErrorAction SilentlyContinue
}

rm -Force -Recurse *.nupkg -ErrorAction SilentlyContinue
Copy-Item template Perspex.Xaml.Behaviors -Recurse
sv lib "Perspex.Xaml.Behaviors\lib\portable-windows8+net45"

mkdir $lib -ErrorAction SilentlyContinue

Copy-Item ..\src\Perspex.Xaml.Interactivity\bin\Release\Perspex.Xaml.Interactivity.dll $lib
Copy-Item ..\src\Perspex.Xaml.Interactivity\bin\Release\Perspex.Xaml.Interactivity.xml $lib
Copy-Item ..\src\Perspex.Xaml.Interactions\bin\Release\Perspex.Xaml.Interactions.dll $lib
Copy-Item ..\src\Perspex.Xaml.Interactions\bin\Release\Perspex.Xaml.Interactions.xml $lib

foreach($pkg in $Packages)
{
    (gc Perspex.Xaml.Behavior\$pkg.nuspec).replace('#VERSION#', $args[0]) | sc $pkg\$pkg.nuspec
}

foreach($pkg in $Packages)
{
    nuget.exe pack $pkg\$pkg.nuspec
}

foreach($pkg in $Packages)
{
    rm -Force -Recurse .\$pkg
}