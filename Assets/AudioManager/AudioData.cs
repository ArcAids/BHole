using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSettings
{
    [CreateAssetMenu()]
    public class AudioData : ScriptableObject
    {
        [SerializeField]
        AudioMixer mixer;
        [SerializeField]
        AudioMixerGroup masterGroup;
        [SerializeField]
        string exposedMasterVolumeName = "MasterVolume";
        [SerializeField]
        string exposedMusicVolumeName = "MusicVolume";
        [SerializeField]
        string exposedSFXVolumeName = "SFXVolume";

        [SerializeField]
        bool muted;
        [SerializeField]
        float musicVolume;
        [SerializeField]
        float masterVolume;
        [SerializeField]
        float sfxVolume;
        [SerializeField]
        AnimationCurve volumeCurve;
        public bool Muted { get => muted; set { MuteAudio(value); } }
        public float MusicVolume { get => musicVolume; set { SetMusicVolume(value); } }
        public float MasterVolume { get => masterVolume; set { SetMasterVolume(value); } }
        public float SFXVolume { get => sfxVolume; set { SetSFXVolume(value); } }

        List<IOnMuteCallback> callbacks = new List<IOnMuteCallback>();

        bool arePropertiesLoaded=false;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void LoadAudioSettings()
        {
            bool isMute=muted;
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            SFXVolume = sfxVolume;
            Muted = isMute;
        }

        void MuteAudio(bool muted)
        {
            if(!muted && MasterVolume==0)
            {
                MasterVolume = 0.2f;
                return;
            }
            this.muted = muted;
            mixer.SetFloat(exposedMasterVolumeName, muted ? -80 : GetActualValue(MasterVolume));
            foreach (var reciever in callbacks)
            {
                reciever.AudioMuted(muted);
            }
            Debug.Log("muted"+muted);
        }

        void SetMasterVolume(float value)
        {
            masterVolume = value;

            mixer.SetFloat(exposedMasterVolumeName, GetActualValue(value));
            if (value == 0)
                Muted = true;
            if (Muted && value > 0)
                Muted = false;
        }

        void SetMusicVolume(float value)
        {
            musicVolume = value;
            mixer.SetFloat(exposedMusicVolumeName, GetActualValue(value));
        }

        void SetSFXVolume(float value)
        {
            sfxVolume = value;
            mixer.SetFloat(exposedSFXVolumeName, GetActualValue(value));
        }

        float GetActualValue(float input)
        {
            return volumeCurve.Evaluate(input);
        }

        public void RegisterCallBack(IOnMuteCallback reciever)
        {
            callbacks.Add(reciever);
        }
        public void DeRegisterCallback(IOnMuteCallback reciever)
        {
            callbacks.Remove(reciever);
        }
    }

    public interface IOnMuteCallback
    {
        void AudioMuted(bool muted);
    }
}