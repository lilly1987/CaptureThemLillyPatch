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
                Faction faction = null;
                string name = null;
                bool? Humanlike = null;
                bool map = false;
                bool designationManager = false;
                try
                {
                    var pawn = ___pawn;
                    if (pawn == null)
                    {
                        MyLog.Warning("pawn == null");
                        return;
                    }
                    if (pawn.Name!=null)
                        name = pawn.Name.ToStringFull;

                    if (pawn.Faction == null)
                    {
                        MyLog.Warning($"{name}/Faction == null", print: Settings.debugMode);
                        return;
                    }
                    else
                    {
                        faction = pawn.Faction;
                        MyLog.Message($"{name}/{pawn.Faction}", print: Settings.debugMode);
                    }
                    if (pawn.RaceProps == null)
                    {
                        MyLog.Warning($"{name}/RaceProps == null", print: Settings.debugMode);
                        return;
                    }
                    else
                    {
                        Humanlike=pawn.RaceProps.Humanlike;
                        MyLog.Message($"{name}/{pawn.RaceProps.Humanlike}", print: Settings.debugMode);
                    }
                    if (pawn.Map == null)
                    {
                        MyLog.Warning($"{name}/Map == null", print: Settings.debugMode);
                        return;
                    }
                    else
                    {
                        map = true;
                    }
                    if (pawn.Map.designationManager == null)
                    {
                        MyLog.Warning($"{name}/Map.designationManager == null", print: Settings.debugMode);
                        return;
                    }
                    else
                    {
                        designationManager=true;
                        MyLog.Message($"{name}/{pawn.Map}", print: Settings.debugMode);
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
                    MyLog.Error($"{faction}/{name}/{Humanlike}/{map}/{designationManager}");
                }
                finally
                {
                }

            }
        }
    }

}
