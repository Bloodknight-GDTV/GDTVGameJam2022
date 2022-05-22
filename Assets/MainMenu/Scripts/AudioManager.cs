using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string firstPlay = "FirstPlay";
    private static readonly string BGMPrefs = "BGMPrefs";
    private static readonly string SFXPrefs = "SFXPrefs";

    private int firstPlayInt;
    public Slider sliderBGM, sliderSFX;
    private float BGMFloat, SFXFloat;

    public AudioSource BGMAudio;
    public AudioSource[] SFXAudio;


    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(firstPlay);

        if(firstPlayInt==0){
            BGMFloat = 1.0f;
            SFXFloat = 1.0f;
            sliderBGM.value = BGMFloat;
            sliderSFX.value = SFXFloat;

            PlayerPrefs.SetFloat(BGMPrefs, BGMFloat);
            PlayerPrefs.SetFloat(SFXPrefs, SFXFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
        }
        else{
            BGMFloat = PlayerPrefs.GetFloat(BGMPrefs);
            SFXFloat = PlayerPrefs.GetFloat(SFXPrefs);

            sliderBGM.value = BGMFloat;
            sliderSFX.value = SFXFloat;
        }
    }

    public void SaveAudioPrefs(){
        PlayerPrefs.SetFloat(BGMPrefs, sliderBGM.value);
        PlayerPrefs.SetFloat(SFXPrefs, sliderSFX.value);
    }

    private void OnApplicationFocus(bool focusStatus) {
        if (!focusStatus)
        {
            SaveAudioPrefs();
        }
    }

    public void UpdateAudio()
    {
        BGMAudio.volume = sliderBGM.value;

        for (var i = 0; i < SFXAudio.Length; i++)
        {
            SFXAudio[i].volume = sliderSFX.value;
        }
        
    }



}
