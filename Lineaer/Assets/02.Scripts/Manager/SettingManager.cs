using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    private Slider _textSpeedSlider = null;

    [SerializeField]
    private Slider _bgmVolumeSlider = null;

    [SerializeField]
    private Slider _seVolumeSlider = null;

    [SerializeField]
    private AudioMixer _audioMixer = null;

    public void ChangeTextPrintSpeed()
    {
        NodeManager.Instance.DialogPanel.PrintDelay = _textSpeedSlider.value;
    }

    public void ChangeBgmVolume()
    {
        float volume = _bgmVolumeSlider.value;
        if (volume == 0)
        {
            _audioMixer.SetFloat("BGMVolume", -80);
        }
        else
        {
            _audioMixer.SetFloat("BGMVolume", volume);
        }
    }

    public void ChangeSeVolume()
    {
        float volume = _seVolumeSlider.value;
        if (volume == 0)
        {
            _audioMixer.SetFloat("SEVolume", -80);
        }
        else
        {
            _audioMixer.SetFloat("SEVolume", volume);
        }
    }
}
