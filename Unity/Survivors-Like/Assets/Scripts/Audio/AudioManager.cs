using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// BGM/SFX ����� ���� ������ ���.
/// - AudioMixer �Ķ����(MasterVol/BGMVol/SFXVol) ����
/// - BGM�� ����, SFX�� OneShot
/// - �����̴�(0~1)�κ��� dB ��ȯ �� SetFloat
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer & Sources")]
    public AudioMixer masterMixer;   // MasterMixer ���� ����
    public AudioSource bgmSource;    // Output= BGM �׷�
    public AudioSource sfxSource;    // Output= SFX �׷�

    [Header("�ʱ� ���� (0~1)")]
    public float masterVolume = 0.8f;
    public float bgmVolume = 0.8f;
    public float sfxVolume = 0.8f;

    [Header("BGM Ŭ��(����)")]
    public AudioClip startBGM;       // ���� �� ����� �����(������)

    public GameObject audioPanel;

    public Slider sliderMaster;
    public Slider sliderBGM;
    public Slider sliderSFX;

    [System.Serializable]
    public class SFXEntry
    {
        public string key;           // "jump", "coin" ��
        public AudioClip clip;
    }
    [Header("SFX ���")]
    public SFXEntry[] sfxList;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ���� ���� ����
        //SetMasterVolume(masterVolume);
        //SetBGMVolume(bgmVolume);
        //SetSFXVolume(sfxVolume);

        //HideAudioPanel()
    }

    void Start()
    {
        if (startBGM != null)
        {
            PlayBGM(startBGM);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            if (audioPanel != null)
            {
                audioPanel.SetActive(true);
            }
        }
    }

    public void SaveVolumeData()
    {
        SaveSystem.SaveVolume(masterVolume, bgmVolume, sfxVolume);
        HideAudioPanel();
    }

    public void HideAudioPanel()
    {
        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }
    }

    // --------- ���� ���� (Slider OnValueChanged�� ����) ---------

    public void SetMasterVolume(float linear01)
    {
        float dB = LinearToDecibel(linear01);
        masterMixer.SetFloat("MasterVol", dB);
        masterVolume = linear01;
    }

    public void SetBGMVolume(float linear01)
    {
        float dB = LinearToDecibel(linear01);
        masterMixer.SetFloat("BGMVol", dB);
        bgmVolume = linear01;
    }

    public void SetSFXVolume(float linear01)
    {
        float dB = LinearToDecibel(linear01);
        masterMixer.SetFloat("SFXVol", dB);
        sfxVolume = linear01;
    }

    private float LinearToDecibel(float linear01)
    {
        // 0�� -80dB(���� ����) ó��
        if (linear01 <= 0.0001f)
        {
            return -80f;
        }
        else
        {
            return 20f * Mathf.Log10(linear01);
        }
    }

    // ---------------- ��� API ----------------

    /// <summary>BGM�� ��ü�ؼ� ���� ���.</summary>
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null)
        {
            return;
        }

        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    /// <summary>��ϵ� Ű�� ȿ���� ���.</summary>
    public void PlaySFX(string key)
    {
        if (sfxSource == null)
        {
            return;
        }

        AudioClip clip = FindSFX(key);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private AudioClip FindSFX(string key)
    {
        if (sfxList != null)
        {
            for (int i = 0; i < sfxList.Length; i++)
            {
                if (sfxList[i] != null)
                {
                    if (sfxList[i].key == key)
                    {
                        return sfxList[i].clip;
                    }
                }
            }
        }
        return null;
    }

    public void UpdateSlider()
    {
        sliderMaster.value = masterVolume;
        sliderBGM.value = bgmVolume;
        sliderSFX.value = sfxVolume;
    }
}