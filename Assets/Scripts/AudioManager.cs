using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer mixer;

    private Slider volumeSlider;
    private const string MixerKey = "MusicVol";
    private const string SaveKey = "GameMusicVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            float savedVolume = PlayerPrefs.GetFloat(SaveKey, 0.8f);
            ApplyVolume(savedVolume);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetVolume(float sliderValue)
    {
        ApplyVolume(sliderValue);
        PlayerPrefs.SetFloat(SaveKey, sliderValue);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float value)
    {
        float db = Mathf.Log10(value) * 20;
        mixer.SetFloat(MixerKey, db);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject sliderObj = GameObject.FindGameObjectWithTag("VolumeSlider");

        if (sliderObj != null)
        {
            volumeSlider = sliderObj.GetComponent<Slider>();
            float savedVolume = PlayerPrefs.GetFloat(SaveKey, 0.8f);
            volumeSlider.value = savedVolume;

            volumeSlider.onValueChanged.RemoveListener(SetVolume);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            volumeSlider = null;
        }
    }
}
