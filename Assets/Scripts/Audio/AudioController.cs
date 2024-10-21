using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider _bgmSlider, _sfxSlider;

    private void Start() {
        if (_bgmSlider != null) {
            AudioManager.Instance.BGMVolume(_bgmSlider.value);
        }
        if (_sfxSlider != null) {
            AudioManager.Instance.SFXVolume(_sfxSlider.value);
        }
    }

    public void ToggleBGM()
    {
        AudioManager.Instance.ToggleBGM();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void BGMVolume()
    {
        AudioManager.Instance.BGMVolume(_bgmSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }
}
