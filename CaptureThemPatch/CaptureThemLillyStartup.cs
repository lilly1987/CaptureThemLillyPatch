using HarmonyLib;
using Verse;

namespace Lilly
{
    // Mod 다음에 StaticConstructorOnStartup 실행
    //[StaticConstructorOnStartup]
    public static class CaptureThemLillyStartup
    {

        static CaptureThemLillyStartup()
        {
            MyLog.Warning($"ST");
            CaptureThemLillyPatch.Patch();
            MyLog.Warning($"ED");
        }
    }
}
