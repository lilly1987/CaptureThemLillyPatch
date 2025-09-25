using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Lilly.CaptureThem
{
    // Mod 다음에 StaticConstructorOnStartup 실행
    public class ModUI : Mod
    {
        public static Settings settings; 
        public static ModUI ui;

        public ModUI(ModContentPack content) : base(content)
        {
            MyLog.Message($"ST");

            ui = this;
            settings = GetSettings<Settings>();// 주의. MainSettings의 patch가 먼저 실행됨            

            MyLog.Message($"ED");
        }

        public override string SettingsCategory()
        {
            return "Capture Them Patch Lilly".Translate();
        }

        
        Vector2 scrollPosition;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            var rect= new Rect(0, 0, inRect.width - 16, 1000);
            Widgets.BeginScrollView(inRect, ref scrollPosition, rect);
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(rect);

            listing.CheckboxLabeled("Debug Mode".Translate(), ref Settings.debugMode, ".");
            listing.CheckboxLabeled("Patch".Translate(), ref Settings.onPatch, ".");

            listing.GapLine();
            listing.End();
            Widgets.EndScrollView();
        }

        static string tmp;

        public static void TextFieldNumeric<T>(Listing_Standard listing, ref T num, string label = "", string tipSignal = "") where T : struct
        {
            listing.Label(label.Translate(), tipSignal: tipSignal.Translate());
            tmp = num.ToString();
            listing.TextFieldNumeric<T>(ref num, ref tmp);
        }
    }
}
