using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace jayounnnn_HeroBrew
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<AudioSource> sfxSources = new List<AudioSource>();
        [SerializeField] private int sfxPoolSize = 10;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip musicMainMenu;
        [SerializeField] private AudioClip musicMainBase;

        [Header("Sound Effects")]
        [SerializeField] private List<SoundEntry> soundEffectEntries = new List<SoundEntry>();

        private Dictionary<SoundType, AudioClip> sfxDict = new Dictionary<SoundType, AudioClip>();

        public enum SoundType
        {
            UI_Click,
            Build_Placed,
            Build_Cancel,
            Collect_Gold,
            Collect_Crystal,
            Collect_Stamina,
            Rotate,
            Error,
            // Add more as needed
        }

        [System.Serializable]
        public class SoundEntry
        {
            public SoundType type;
            public AudioClip clip;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var entry in soundEffectEntries)
                sfxDict[entry.type] = entry.clip;

            // Initialise audio pool
            for (int i = 0; i < sfxPoolSize; i++)
            {
                var sfx = gameObject.AddComponent<AudioSource>();
                sfx.playOnAwake = false;
                sfxSources.Add(sfx);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            PlayMusicForScene(SceneManager.GetActiveScene().name);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusicForScene(scene.name);
        }

        public void PlayMusicForScene(string sceneName)
        {
            if (sceneName == "MainMenu")
                PlayMusic(musicMainMenu);
            else if (sceneName == "MainBase")
                PlayMusic(musicMainBase);
            // Add more scene checks if needed
        }

        public void PlayMusic(AudioClip clip)
        {
            if (musicSource == null) return;
            if (musicSource.clip == clip) return;

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlaySFX(SoundType type)
        {
            if (!sfxDict.TryGetValue(type, out var clip))
            {
                Debug.LogWarning("[AudioManager] Missing sound: " + type);
                return;
            }

            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.clip = clip;
                    source.volume = 1f; // Optionally customise volume per type
                    source.Play();
                    return;
                }
            }

            Debug.LogWarning("[AudioManager] No available audio sources for SFX pool.");
        }
    }
}