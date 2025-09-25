using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Lilly
{
    public class CaptureThemLillySettings : ModSettings
    {
        public static CaptureThemLillySettings settings;

        public static bool debugMode = false;
        public static bool onPatch = true;

        public CaptureThemLillySettings()
        {
            MyLog.Message($"ST");
            settings = this;
            MyLog.Message($"ED");
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref debugMode, "debugMode", false);
            Scribe_Values.Look(ref onPatch, "onPatch", true);

            if (onPatch)
            {
                CaptureThemLillyPatch.Patch();
            }
            else
            {
                CaptureThemLillyPatch.UnPatch();
            }
        }


    }

}
