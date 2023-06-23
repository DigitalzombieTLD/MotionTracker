using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2Cpp;
using UnityEngine.UI;

namespace MotionTracker
{
    public class PingManager : MonoBehaviour
    {
        public PingManager(IntPtr intPtr) : base(intPtr)
        {
        }
        public enum AnimalType {  Crow, Rabbit, Stag, Doe, Wolf, Timberwolf, Bear, Moose, PuffyBird };

        public static bool isVisible = false;
        public static PingManager instance;

        public RectTransform iconContainer, radarUI;
        public Image backgroundImage;
        public Canvas trackerCanvas;

        public bool applyRotation = true;
        public static bool inMenu = false;
       

        public void ClearIcons()
        {
            Image[] icons = iconContainer.transform.GetComponentsInChildren<Image>();

            foreach(Image icon in icons) 
            { 
                Destroy(icon.gameObject);
            }
        }

        public void Update()
        {
            if(AllowedToBeVisible())
            {
                SetVisible(true);
            }
            else
            {
                SetVisible(false);
            }
        }

        public bool AllowedToBeVisible()
        {           
            if (!Settings.options.enableMotionTracker)
            {
                return false;
            }

            if (!MotionTrackerMain.modSettingPage)
            {
                MotionTrackerMain.modSettingPage = GameObject.Find("Mod settings grid (Motion Tracker)");               
            }

            if (MotionTrackerMain.modSettingPage)
            {
                if (MotionTrackerMain.modSettingPage.active)
                {
                    return true;
                }
            }

            if (inMenu)
            {
                return false;
            }

            if (Settings.options.displayStyle == Settings.DisplayStyle.Toggle)
            {
                if(!Settings.toggleBool)
                {
                    return false;                    
                }                
            }

            if (!GameManager.GetVpFPSPlayer())
            {
                return false;
            }

            if (!GameManager.GetWeatherComponent())
            {
                return false;
            }


            if (Settings.options.onlyOutdoors)
            {
                if(GameManager.GetWeatherComponent().IsIndoorEnvironment())
                {
                    return false; 
                }
            }

          
            return true;
        }
       
        private void SetVisible(bool visible)
        {
            if (isVisible == visible)
            {
                return;
            }

            if (visible)
            {
                trackerCanvas.enabled = true;
                isVisible = true;
            }
            else
            {
                trackerCanvas.enabled = false;
                isVisible = false;
            }
        }

        public void Awake()
        {
            instance = this;

            trackerCanvas = MotionTrackerMain.trackerObject.transform.FindChild("Canvas").GetComponent<Canvas>();

            radarUI = trackerCanvas.transform.FindChild("RadarUI").GetComponent<RectTransform>();
            radarUI.localScale = new Vector3(Settings.options.scale, Settings.options.scale, Settings.options.scale);

            iconContainer = radarUI.transform.FindChild("IconContainer").GetComponent<RectTransform>();

            backgroundImage = radarUI.transform.FindChild("Background").GetComponent<Image>();
            backgroundImage.color = new Color(1f, 1f, 1f, Settings.options.opacity);
            
            SetOpacity(Settings.options.opacity);
            Scale(Settings.options.scale);

            trackerCanvas.enabled = true;
            isVisible = true;
        }

        public void Scale(float scale)
        {
            if (radarUI)
            {
                radarUI.localScale = new Vector3(scale, scale, scale);
            }
        }

        public void SetOpacity(float opacity)
        {
            if (backgroundImage)
            {
                backgroundImage.color = new Color(1f, 1f, 1f, opacity);
            }
        }
    }
}