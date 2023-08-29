using System.Runtime.CompilerServices;
using VerifyTests;

public static class ModuleInit
{
    [ModuleInitializer]
    public static void InitOther()
    {
        VerifierSettings.InitializePlugins();
        VerifyImageMagick.RegisterComparers(0.04);
        VerifierSettings.UniqueForOSPlatform();
    }
}
