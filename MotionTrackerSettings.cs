using UnityEngine;
using ModSettings;
using MelonLoader;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace MotionTracker
{
    internal class MotionTrackerSettings : JsonModSettings
    {     
		[Section("General")]

        [Name("Enable Motion Tracker")]
        [Description("Enable/Disable Motion Tracker")]
        public bool enableMotionTracker = true;

        [Name("Visibility")]
        [Description("Always visible / Visible on key toggle")]
        public Settings.DisplayStyle displayStyle = Settings.DisplayStyle.AlwaysOn;

        [Name("Toggle Key")]
        [Description("Toggle visibility on keypress")]
        public KeyCode toggleKey = KeyCode.Keypad0;

        [Name("Only outdoors")]
        [Description("Only enables overlay while outdoors")]
        public bool onlyOutdoors = true;

        [Name("Detection Range")]
        [Description("Range to detect motion")]
        [Slider(0, 800)]
        public int detectionRange = 100;

        [Name("Scale")]
        [Description("Scale of motion detector overlay")]
        [Slider(0, 4)]
        public float scale = 1f;

        [Name("Background Opacity")]
        [Description("Opacity of motion detector overlay")]
        [Slider(0, 1)]
        public float opacity = 0.7f;

        [Section("Spraypaint")]

        [Name("Show Spraypaint Markers")]
        [Description("Enable / Disable")]
        public bool showSpraypaint = true;

        [Name("Spraypaint Icon Scale")]
        [Description("Scale of spraypaint icons")]
        [Slider(0.2f, 5)]
        public float spraypaintScale = 2.0f;

        [Name("Spraypaint Opacity")]
        [Description("Opacity of spraypaint icons")]
        [Slider(0, 1)]
        public float spraypaintOpacity = 0.8f;

        [Section("Wildlife")]

        [Name("Animal Icon Scale")]
        [Description("Scale of animal icons")]
        [Slider(0, 5)]
        public float animalScale = 3.5f;

        [Name("Animal Icon Opacity")]
        [Description("Opacity of animal icons")]
        [Slider(0, 1)]
        public float animalOpacity = 0.8f;

        [Name("Show Crows")]
        [Description("Track motion of crows")]
        public bool showCrows = true;

        [Name("Show Rabbits")]
        [Description("Track motion of rabbits")]
        public bool showRabbits = true;

        [Name("Show Stags")]
        [Description("Track motion of stags")]
        public bool showStags = true;

        [Name("Show Does")]
        [Description("Track motion of does")]
        public bool showDoes = true;

        [Name("Show Wolves")]
        [Description("Track motion of wolves")]
        public bool showWolves = true;

        [Name("Show Timberwolves")]
        [Description("Track motion of timberwolves")]
        public bool showTimberwolves = true;

        [Name("Show Bears")]
        [Description("Track motion of bears")]
        public bool showBears = true;

        [Name("Show Moose")]
        [Description("Track motion of moose")]
        public bool showMoose = true;

        [Name("Show Puffy Birds")]
        [Description("Track motion of puffy birds")]
        public bool showPuffyBirds = true;


        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();

            if (PingManager.instance)
            {  
                PingManager.instance.SetOpacity(Settings.options.opacity);
                PingManager.instance.Scale(Settings.options.scale);

                Settings.animalScale = new Vector3(Settings.options.animalScale, Settings.options.animalScale, Settings.options.animalScale);
                Settings.spraypaintScale = new Vector3(Settings.options.spraypaintScale, Settings.options.spraypaintScale, Settings.options.spraypaintScale);
                Settings.animalColor = new Color(1, 1, 1, Settings.options.animalOpacity);
                Settings.spraypaintColor = new Color(0.62f, 0.29f, 0.0f, Settings.options.spraypaintOpacity);
            }
        }
    }

    internal static class Settings
    {
        public static MotionTrackerSettings options;
        public static Vector3 animalScale;
        public static Vector3 spraypaintScale;

        public static Color animalColor;
        public static Color spraypaintColor;

        public static bool toggleBool = false;

        public enum DisplayStyle
        {
            AlwaysOn, Toggle
        };

        public static void OnLoad()
        {
            options = new MotionTrackerSettings();
            options.AddToModSettings("Motion Tracker");

            animalScale = new Vector3(options.animalScale, options.animalScale, options.animalScale);
            spraypaintScale = new Vector3(options.spraypaintScale, options.spraypaintScale, options.spraypaintScale);
            animalColor = new Color(1, 1, 1, options.animalOpacity);
            spraypaintColor = new Color(0.62f, 0.29f, 0.0f, options.spraypaintOpacity);
        }
    }
}
