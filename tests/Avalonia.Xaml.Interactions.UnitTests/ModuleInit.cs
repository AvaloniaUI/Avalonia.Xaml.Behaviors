using System.Runtime.CompilerServices;
using Argon;
using VerifyTests;

public static class ModuleInit
{
    [ModuleInitializer]
    public static void InitOther()
    {
        VerifierSettings.InitializePlugins();
        VerifierSettings.AddExtraSettings(_=>_.PreserveReferencesHandling = PreserveReferencesHandling.Objects );
    }
}
