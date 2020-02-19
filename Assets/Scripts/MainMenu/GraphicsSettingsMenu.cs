using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameSettingsUI
{
    public class GraphicsSettingsMenu : MonoBehaviour
    {
        [SerializeField]
        TMP_Dropdown qualityDropBox;
        [SerializeField]
        TMP_Dropdown resolutionDropBox;
        

        private void Awake()
        {
            UpdateGraphicsQualityDropBox();
            UpdateResolutionDropBox();
        }
        
        void UpdateResolutionDropBox()
        {
            List<string> qualityOptions = new List<string>();

            resolutionDropBox.ClearOptions();
            int currentResolutionIndex = 0;
            bool found = false;
            foreach (var item in Screen.resolutions)
            {
                if (!found)
                {
                    if (item.height == Screen.currentResolution.height && item.width == Screen.currentResolution.width)
                        found = true;
                    else
                        currentResolutionIndex++;
                }
                qualityOptions.Add(item.ToString());
            }
            resolutionDropBox.AddOptions(qualityOptions);
            resolutionDropBox.value = currentResolutionIndex;
        }

        void UpdateGraphicsQualityDropBox()
        {
            List<string> qualityOptions = new List<string>();
            qualityDropBox.ClearOptions();
            foreach (var item in QualitySettings.names)
            {
                qualityOptions.Add(item);
            }
            qualityDropBox.AddOptions(qualityOptions);
            qualityDropBox.value = QualitySettings.GetQualityLevel();

            qualityOptions.Clear();
        }

        public void FullScreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }

        public void SetGraphicsLevel(int level)
        {
            QualitySettings.SetQualityLevel(level);
        }
        public void SetResolution(int level)
        {
            Screen.SetResolution(Screen.resolutions[level].width, Screen.resolutions[level].height, Screen.fullScreen);
        }
    }
}
