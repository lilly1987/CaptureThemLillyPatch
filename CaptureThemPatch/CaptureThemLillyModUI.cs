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

namespace Lilly
{
    // Mod 다음에 StaticConstructorOnStartup 실행
    public class CaptureThemLillyModUI : Mod
    {
        public static CaptureThemLillySettings settings; 
        public static CaptureThemLillyModUI ui;

        public CaptureThemLillyModUI(ModContentPack content) : base(content)
        {
            MyLog.Warning($"ST");

            ui = this;
            settings = GetSettings<CaptureThemLillySettings>();// 주의. MainSettings의 patch가 먼저 실행됨            

            MyLog.Warning($"ED");
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

            listing.CheckboxLabeled("Debug Mode".Translate(), ref CaptureThemLillySettings.debugMode, ".");
            listing.CheckboxLabeled("Patch".Translate(), ref CaptureThemLillySettings.onPatch, ".");

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
