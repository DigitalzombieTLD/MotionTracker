using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection;
using System.Collections;
using Il2Cpp;
using System.Reflection;


namespace MotionTracker
{
	public class MotionTrackerMain : MelonMod
	{
        public static AssetBundle assetBundle;
        public static GameObject motionTrackerParent;
        public static PingManager activePingManager;

        public static GameObject trackerPrefab;
        public static GameObject trackerObject;

        public static GameObject modSettingPage;

        public static Dictionary<PingManager.AnimalType, GameObject> animalPingPrefabs = new Dictionary<PingManager.AnimalType, GameObject>();
        public static Dictionary<ProjectileType, GameObject> spraypaintPingPrefabs = new Dictionary<ProjectileType, GameObject>();

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<TweenManager>();
            ClassInjector.RegisterTypeInIl2Cpp<PingManager>();
            ClassInjector.RegisterTypeInIl2Cpp<PingComponent>();
            LoadEmbeddedAssetBundle();

            MotionTracker.Settings.OnLoad();
        }

        public static void LoadEmbeddedAssetBundle()
        {
            MemoryStream memoryStream;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MotionTracker.Resources.motiontracker");
            memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            assetBundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
            if(sceneName.Contains("MainMenu"))
            {
                //SCRIPT_InterfaceManager/_GUI_Common/Camera/Anchor/Panel_OptionsMenu/Pages/ModSettings/GameObject/ScrollPanel/Offset/

                PingManager.inMenu = true;


                FirstTimeSetup();
            }
            else if (sceneName.Contains("SANDBOX") && motionTrackerParent)
            {
                if (PingManager.instance)
                {
                    PingManager.instance.ClearIcons();
                    PingManager.inMenu = false;
                }
            }
        }

        public void FirstTimeSetup()
        {
            if (!motionTrackerParent)
            {
                motionTrackerParent = new GameObject("MotionTracker");
                trackerObject = UnityEngine.Object.Instantiate(assetBundle.LoadAsset<GameObject>("MotionTracker"), motionTrackerParent.transform);
                GameObject.DontDestroyOnLoad(motionTrackerParent);

                activePingManager = motionTrackerParent.AddComponent<PingManager>();

                GameObject prefabSafe = new GameObject("PrefabSafe");
                prefabSafe.transform.parent = motionTrackerParent.transform;
                animalPingPrefabs = new Dictionary<PingManager.AnimalType, GameObject>();
                animalPingPrefabs.Add(PingManager.AnimalType.Crow, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("crow"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Rabbit, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("rabbit"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Wolf, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("wolf"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Timberwolf, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("timberwolf"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Bear, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("bear"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Moose, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("moose"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Stag, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("stag"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.Doe, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("doe"), prefabSafe.transform));
                animalPingPrefabs.Add(PingManager.AnimalType.PuffyBird, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("ptarmigan"), prefabSafe.transform));

                spraypaintPingPrefabs = new Dictionary<ProjectileType, GameObject>();
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Direction, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Direction"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Clothing, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Clothing"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Danger, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Danger"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_DeadEnd, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_DeadEnd"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Avoid, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Avoid"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_FirstAid, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_FirstAid"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_FoodDrink, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_FoodDrink"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_FireStarting, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_FireStarting"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Hunting, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Hunting"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Materials, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Materials"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Storage, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Storage"), prefabSafe.transform));
                spraypaintPingPrefabs.Add(ProjectileType.SprayPaint_Tools, GameObject.Instantiate(assetBundle.LoadAsset<GameObject>("SprayPaint_Tools"), prefabSafe.transform));

                foreach (KeyValuePair<PingManager.AnimalType, GameObject> singlePrefab in animalPingPrefabs)
                {
                    singlePrefab.Value.active = false;
                }

                foreach (KeyValuePair<ProjectileType, GameObject> singlePrefab in spraypaintPingPrefabs)
                {
                    singlePrefab.Value.active = false;
                }

                GameObject.DontDestroyOnLoad(prefabSafe);
            }
        }

        public static GameObject GetAnimalPrefab(PingManager.AnimalType animalType)
        {  
            return animalPingPrefabs[animalType];
        }

        public static GameObject GetSpraypaintPrefab(ProjectileType pingType)
        {
            return spraypaintPingPrefabs[pingType];
        }

        public override void OnUpdate()
		{
            if(Settings.options.displayStyle == Settings.DisplayStyle.Toggle)
            {
                if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.toggleKey))
                {
                    if (PingManager.instance)
                    {
                        Settings.toggleBool = !Settings.toggleBool;                       
                    }
                }
            }       
        }
    }
}