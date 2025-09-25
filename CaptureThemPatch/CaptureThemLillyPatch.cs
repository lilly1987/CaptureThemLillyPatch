using CaptureThem;
using HarmonyLib;
using Lilly;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static System.Collections.Specialized.BitVector32;

namespace Lilly
{
    public static class CaptureThemLillyPatch
    {
        public static string harmonyId = "Lilly.CaptureThemPatch";
        public static Harmony harmony;

        public static void Patch(bool repatch=false)
        {
            if (repatch)
            {
                UnPatch();
            }
            if (harmony == null)
            {
                MyLog.Message($"{harmonyId} Patch ST");
                try
                {
                    harmony = new Harmony(harmonyId);
                    harmony.PatchAll();
                    MyLog.Message($"{harmonyId} Patch Succ", color: "27AE60FF");
                }
                catch (System.Exception e)
                {
                    MyLog.Error($"{harmonyId} Patch Fail");
                    MyLog.Error(e.ToString());
                    MyLog.Error($"{harmonyId} Patch Fail");
                }
                MyLog.Message($"Patch ED");
            }
        }

        public static void UnPatch()
        {
            if (harmony != null)
            {
                MyLog.Message($"{harmonyId} UnPatch ST");
                try
                {
                    harmony.UnpatchAll(harmonyId);
                    harmony = null;
                    MyLog.Message($"{harmonyId} UnPatch Succ",color: "27AE60FF");
                }
                catch (System.Exception e)
                {
                    MyLog.Error($"{harmonyId} UnPatch Fail");
                    MyLog.Error(e.ToString());
                    MyLog.Error($"{harmonyId} UnPatch Fail");
                }
                MyLog.Message($"{harmonyId} UnPatch ED");
            }
        }

        [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
        public class Pawn_HealthTracker_MakeDowned
        {
            public static void Postfix(Pawn ___pawn)
            {
                var pawn = ___pawn;
                MyLog.Message($"MakeDowned {pawn} {pawn?.Faction} {pawn.Faction != Faction.OfPlayer} {!pawn.Faction.Hidden} {pawn.Faction.HostileTo(Faction.OfPlayer)} {!pawn.IsPrisonerOfColony} {pawn.RaceProps.Humanlike}", print: CaptureThemLillySettings.debugMode);
                if (pawn?.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.Hidden &&
                    pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.IsPrisonerOfColony && pawn.RaceProps.Humanlike)
                {
                    ___pawn.Map.designationManager.RemoveAllDesignationsOn(___pawn);
                    ___pawn.Map.designationManager.AddDesignation(new Designation(___pawn, CaptureThemDefOf.CaptureThemCapture));
                }

            }
        }
    }

}
