using UnityEngine;
using UnityEngine.UI;
using AudioSettings;

namespace GameSettingsUI
{
    public class AudioSettingsMenu : MonoBehaviour, IOnMuteCallback
    {
        [SerializeField]
        AudioData audioController;
        [SerializeField]
        Toggle muteToggle;
        [SerializeField]
        Slider masterSlider;
        [SerializeField]
        Slider musicSlider;
        [SerializeField]
        Slider sfxSlider;

        public void UpdateSettings()
        {
            //audioController.LoadAudioSettings();
            bool isMuted = audioController.Muted;
            Debug.Log("isMuted"+isMuted);
            muteToggle.isOn = false;
            masterSlider.value = audioController.MasterVolume;
            musicSlider.value = audioController.MusicVolume;
            sfxSlider.value = audioController.SFXVolume;
            Debug.Log("isMuted"+isMuted);
            muteToggle.isOn = isMuted;
        }

        private void OnEnable()
        {
            UpdateSettings();
            audioController.RegisterCallBack(this);
        }
        private void OnDisable()
        {
            audioController.DeRegisterCallback(this);
        }
        public void AudioMuted(bool muted)
        {
            muteToggle.isOn = muted;
            if(!muted)
            {
                
                masterSlider.fillRect.GetComponent<Image>().color=Color.cyan;
                musicSlider.fillRect.GetComponent<Image>().color=Color.cyan;
                sfxSlider.fillRect.GetComponent<Image>().color=Color.cyan;
            }
            else
            {
                masterSlider.fillRect.GetComponent<Image>().color = Color.blue;
                musicSlider.fillRect.GetComponent<Image>().color =  Color.blue;
                sfxSlider.fillRect.GetComponent<Image>().color =    Color.blue;
            }
        }
    }


}