using CaptureThem;
using HarmonyLib;
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

namespace Lilly.CaptureThem
{
    public static class ModPatch
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
                try
                {

                    var pawn = ___pawn;
                    if (pawn == null)
                    {
                        MyLog.Warning("pawn == null");
                        return;
                    }
                    if (pawn.Faction == null)
                    {
                        MyLog.Warning($"{pawn.Name.ToStringFull} Faction == null");
                        return;
                    }
                    else
                    {
                        MyLog.Message($"{pawn.Name.ToStringFull}/{pawn.Faction}", print: Settings.debugMode);
                    }
                    if (pawn.RaceProps == null)
                    {
                        MyLog.Warning($"{pawn.Name.ToStringFull} RaceProps == null");
                        return;
                    }
                    else
                    {
                        MyLog.Message($"{pawn.Name.ToStringFull}/{pawn.RaceProps.Humanlike}", print: Settings.debugMode);
                    }
                    if (pawn.Map == null)
                    {
                        MyLog.Warning($"{pawn.Name.ToStringFull} Map == null");
                        return;
                    }
                    else
                    {
                        MyLog.Message($"{pawn.Name.ToStringFull}/{pawn.Map}", print: Settings.debugMode);
                    }
                    if (pawn.Faction != Faction.OfPlayer && !pawn.Faction.Hidden &&
                        pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.IsPrisonerOfColony && pawn.RaceProps.Humanlike)
                    {
                        ___pawn.Map.designationManager.RemoveAllDesignationsOn(___pawn);
                        ___pawn.Map.designationManager.AddDesignation(new Designation(___pawn, CaptureThemDefOf.CaptureThemCapture));
                    }
                }
                catch (Exception e)
                {
                    MyLog.Error(e.ToString());
                }

            }
        }
    }

}
