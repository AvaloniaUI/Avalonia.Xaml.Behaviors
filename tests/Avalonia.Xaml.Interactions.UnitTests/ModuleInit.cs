using System.Runtime.CompilerServices;
using VerifyTests;

public static class ModuleInit
{
    [ModuleInitializer]
    public static void InitOther()
    {
        VerifyImageMagick.RegisterComparers(0.02);
        VerifierSettings.InitializePlugins();
        VerifierSettings.UniqueForOSPlatform();
    }
}
