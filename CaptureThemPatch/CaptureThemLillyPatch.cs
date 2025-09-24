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
                MyLog.Warning($"{harmonyId} Patch ST");
                try
                {
                    harmony = new Harmony(harmonyId);
                    harmony.PatchAll();
                    MyLog.Warning($"{harmonyId} Patch Succ", color: "27AE60FF");
                }
                catch (System.Exception e)
                {
                    MyLog.Error($"{harmonyId} Patch Fail");
                    MyLog.Error(e.ToString());
                    MyLog.Error($"{harmonyId} Patch Fail");
                }
                MyLog.Warning($"Patch ED");
            }
        }

        public static void UnPatch()
        {
            if (harmony != null)
            {
                MyLog.Warning($"{harmonyId} UnPatch ST");
                try
                {
                    harmony.UnpatchAll(harmonyId);
                    harmony = null;
                    MyLog.Warning($"{harmonyId} UnPatch Succ",color: "27AE60FF");
                }
                catch (System.Exception e)
                {
                    MyLog.Error($"{harmonyId} UnPatch Fail");
                    MyLog.Error(e.ToString());
                    MyLog.Error($"{harmonyId} UnPatch Fail");
                }
                MyLog.Warning($"{harmonyId} UnPatch ED");
            }
        }

        [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
        public class Pawn_HealthTracker_MakeDowned
        {
            public static void Postfix(Pawn ___pawn)
            {
                var pawn = ___pawn;

                if (pawn?.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.Hidden &&
                    !pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.IsPrisonerOfColony && pawn.RaceProps.Humanlike)
                {
                    ___pawn.Map.designationManager.RemoveAllDesignationsOn(___pawn);
                    ___pawn.Map.designationManager.AddDesignation(new Designation(___pawn, CaptureThemDefOf.CaptureThemCapture));
                    MyLog.Message(
                        $"{ "MessageCapturingWillAngerFaction".Translate(pawn.Named("PAWN")).AdjustedFor(pawn)}, { pawn}, { MessageTypeDefOf.CautionInput}");
                }

            }
        }
    }

}
