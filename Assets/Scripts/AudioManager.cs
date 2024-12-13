using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    [Header("Аудио компоненты")]
    public AudioSource backgroundMusic; 
    public AudioSource soundEffect;     

    [Header("Слайдеры для управления громкостью")]
    public Slider musicSlider;          
    public Slider soundSlider;         

    private static AudioManager instance;

    void Awake()
    {
        // Проверяем, уже существует ли экземпляр AudioManager
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // Уничтожаем дублирующийся объект
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Debug.Log("AudioManager instance created");
    }

    void Start()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true; 
            backgroundMusic.Play();
        }

        RestoreVolume();

        if (musicSlider) 
        {
            musicSlider.value = backgroundMusic.volume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (soundSlider)
        {
            soundSlider.value = soundEffect.volume;
            soundSlider.onValueChanged.AddListener(SetSoundVolume);
        }
    }

    private void RestoreVolume()
    {
        if (backgroundMusic != null)
        {
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); 
            backgroundMusic.volume = savedMusicVolume;
        }

        if (soundEffect != null)
        {
            float savedSoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
            soundEffect.volume = savedSoundVolume;
        }
    }

    private void SetMusicVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }
    }

    private void SetSoundVolume(float volume)
    {
        if (soundEffect != null)
        {
            soundEffect.volume = volume;
            PlayerPrefs.SetFloat("SoundVolume", volume);
            PlayerPrefs.Save();
        }
    }

    public void PlaySoundEffect()
    {
        if (soundEffect != null && !soundEffect.isPlaying)
        {
            soundEffect.Play();
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }
}
