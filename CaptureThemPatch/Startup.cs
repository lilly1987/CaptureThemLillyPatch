using HarmonyLib;
using Verse;

namespace Lilly.CaptureThem
{
    // Mod 다음에 StaticConstructorOnStartup 실행
    //[StaticConstructorOnStartup]
    public static class Startup
    {

        static Startup()
        {
            MyLog.Message($"ST");
            ModPatch.Patch();
            MyLog.Message($"ED");
        }
    }
}
