using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2Cpp;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine.UI;

namespace MotionTracker
{
	public class PingComponent : MonoBehaviour
	{
        public PingComponent(IntPtr intPtr) : base(intPtr)
        {
        }
               
        public GameObject attachedGameObject;     
        public PingManager.AnimalType animalType;
        public ProjectileType spraypaintType;
        public PingCategory assignedCategory;
     
        public CanvasGroup canvasGroup;
        public GameObject iconObject;
        public bool isInitialized = false;
        public Image iconImage;
       

        public enum PingCategory
        {
          None, Animal, Spraypaint
        };

        public RectTransform rectTransform;
        public bool clampOnRadar = false;
        public static GameObject playerObject;




        [HideFromIl2Cpp]
        public void CreateIcon()
        {
            if(assignedCategory == PingCategory.Animal)
            {
                iconObject = Instantiate(MotionTrackerMain.GetAnimalPrefab(animalType));
                iconImage = iconObject.GetComponent<Image>();
                iconImage.color = Settings.animalColor;
            }
            else if (assignedCategory == PingCategory.Spraypaint)
            {
                iconObject = Instantiate(MotionTrackerMain.GetSpraypaintPrefab(spraypaintType));
                iconImage = iconObject.GetComponent<Image>();
                iconImage.color = Settings.spraypaintColor;
            }

            iconObject.transform.SetParent(PingManager.instance.iconContainer.transform, false);
            iconObject.active = true;
            canvasGroup = iconObject.GetComponent<CanvasGroup>();
            rectTransform = iconObject.GetComponent<RectTransform>();
        }

        [HideFromIl2Cpp]
        public void DeleteIcon()
        {
            if (iconObject)
            {
                GameObject.Destroy(iconObject);
            }
        }

        [HideFromIl2Cpp]
        public bool AllowedToShow()
        {
            if (assignedCategory == PingCategory.Animal)
            {
                if (animalType == PingManager.AnimalType.Crow && Settings.options.showCrows)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Rabbit && Settings.options.showRabbits)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Stag && Settings.options.showStags)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Doe && Settings.options.showDoes)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Wolf && Settings.options.showWolves)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Timberwolf && Settings.options.showTimberwolves)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Bear && Settings.options.showBears)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.Moose && Settings.options.showMoose)
                {
                    return true;
                }
                else if (animalType == PingManager.AnimalType.PuffyBird && Settings.options.showPuffyBirds)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (assignedCategory == PingCategory.Spraypaint && Settings.options.showSpraypaint)
            {
                return true;                
            }         

            return false;
        }

        [HideFromIl2Cpp]
        public static void ManualDelete(PingComponent pingComponent)
        {
            if (pingComponent != null)
            {
                pingComponent.DeleteIcon();
                GameObject.Destroy(pingComponent);
            }
        }

        [HideFromIl2Cpp]
        public void SetVisible(bool visibility)
        {
            if (AllowedToShow() && visibility)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
            }
        }

        [HideFromIl2Cpp]       
        public void Initialize(PingManager.AnimalType type)
        {
            attachedGameObject = this.gameObject;
            animalType = type;
            assignedCategory = PingCategory.Animal;
            
            CreateIcon();

            isInitialized = true;
        }

        [HideFromIl2Cpp]
        public void Initialize(ProjectileType type)
        {
            attachedGameObject = this.gameObject;
            spraypaintType = type;
            assignedCategory = PingCategory.Spraypaint;
            

            CreateIcon();

            isInitialized = true;
        }

        [HideFromIl2Cpp]
        private void OnDisable()
        {
            DeleteIcon();
        }

       
        public void Update()
        {
            if (Settings.options.enableMotionTracker && PingManager.isVisible)
            {
                if (SaveGameSystem.m_CurrentGameMode == SaveSlotType.SANDBOX)
                {
                    if (GameManager.GetVpFPSPlayer() != null)
                    {
                        UpdateLocatableIcons();
                    }
                }
            }
        }

        private void UpdateLocatableIcons()
        {
            if (TryGetIconLocation(out var iconLocation))
            {
                SetVisible(true);
                rectTransform.anchoredPosition = iconLocation;

                if(assignedCategory == PingCategory.Spraypaint)
                {
                    if (iconImage.color != Settings.spraypaintColor || rectTransform.localScale != Settings.spraypaintScale)
                    {
                        rectTransform.localScale = Settings.spraypaintScale;
                        iconImage.color = Settings.spraypaintColor;
                    }
                }
                else if (assignedCategory == PingCategory.Animal)
                {
                    if (iconImage.color != Settings.spraypaintColor || rectTransform.localScale != Settings.spraypaintScale)
                    {
                        rectTransform.localScale = Settings.animalScale;
                        iconImage.color = Settings.animalColor;
                    }
                }
            }
            else
            {
                SetVisible(false);
            }
        }

        private bool TryGetIconLocation(out Vector2 iconLocation)
        {
            iconLocation = GetDistanceToPlayer(this);

            float radarSize = GetRadarUISize();

            var scale = radarSize / Settings.options.detectionRange;

            iconLocation *= scale;

            // Rotate the icon by the players y rotation if enabled
            if (PingManager.instance.applyRotation)
            {
                var playerForwardDirectionXZ = new Vector3(0, 0, 0);

                // Get the forward vector of the player projected on the xz plane
                if (GameManager.GetVpFPSPlayer())
                {
                    playerForwardDirectionXZ = Vector3.ProjectOnPlane(GameManager.GetVpFPSPlayer().gameObject.transform.forward, Vector3.up);
                }

                // Create a roation from the direction
                var rotation = Quaternion.LookRotation(playerForwardDirectionXZ);

                // Mirror y rotation
                var euler = rotation.eulerAngles;
                euler.y = -euler.y;
                rotation.eulerAngles = euler;

                // Rotate the icon location in 3D space
                var rotatedIconLocation = rotation * new Vector3(iconLocation.x, 0.0f, iconLocation.y);

                // Convert from 3D to 2D
                iconLocation = new Vector2(rotatedIconLocation.x, rotatedIconLocation.z);
            }

            if (iconLocation.sqrMagnitude < radarSize * radarSize || this.clampOnRadar)
            {
                // Make sure it is not shown outside the radar
                iconLocation = Vector2.ClampMagnitude(iconLocation, radarSize);

                return true;
            }

            return false;
        }

        private float GetRadarUISize()
        {
            return PingManager.instance.iconContainer.rect.width / 2;
        }

        private Vector2 GetDistanceToPlayer(PingComponent locatable)
        {
            if (GameManager.GetVpFPSPlayer() && locatable)
            {
                Vector3 distanceToPlayer = locatable.transform.position - GameManager.GetVpFPSPlayer().gameObject.transform.position;
                return new Vector2(distanceToPlayer.x, distanceToPlayer.z);
            }

            return new Vector2(0, 0);
        }
    }
}