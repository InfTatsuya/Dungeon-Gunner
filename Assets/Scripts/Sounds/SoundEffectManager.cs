using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager>
{
    public int soundsVolume = 8;

    private void Start()
    {
        if (PlayerPrefs.HasKey("soundsVolume"))
        {
            soundsVolume = PlayerPrefs.GetInt("soundsVolume");
        }

        SetSoundsVolume(soundsVolume);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("soundsVolume", soundsVolume);
    }

    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(
            soundEffect.soundPrefab,
            Vector3.zero,
            Quaternion.identity);

        sound.SetSound(soundEffect);

        sound.gameObject.SetActive(true);

        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    private IEnumerator DisableSound(SoundEffect sound, float length)
    {
        yield return new WaitForSeconds(length);
        sound.gameObject.SetActive(false);
    }

    private void SetSoundsVolume(int soundsVolume)
    {
        float muteDecibels = -80f;

        if(soundsVolume == 0)
        {
            GameResources.Instance.soundMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.soundMasterMixerGroup.audioMixer.SetFloat("soundsVolume", 
                HelperUtilities.LinearToDecibels(soundsVolume));
        }
    }

    public void IncreaseSoundVolume()
    {
        int maxSoundVolume = 20;

        if (soundsVolume >= maxSoundVolume) return;

        soundsVolume++;

        SetSoundsVolume(soundsVolume);
    }

    public void DecreaseSoundVolume()
    {
        if (soundsVolume <= 0) return;

        soundsVolume--;

        SetSoundsVolume(soundsVolume);
    }
}
