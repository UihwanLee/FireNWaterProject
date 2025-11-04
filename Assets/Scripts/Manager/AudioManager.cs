using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /*
     * 
     * AudioManager
     * 
     * 오디오 매니저는 SoundSource를 오브젝트 폴링 방식으로 관리
     * Setting 메뉴에서 volume 값을 바꿀 수 있도록 [0f,1f] 제한
     * 
     */

    public static AudioManager instance;

    [Header("Setting")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField][Range(0f, 1f)] private float musicVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;

    [Header("SoundSourceList")]
    [SerializeField] private SoundSource soundSourcePrefab;
    [SerializeField] private SoundSource[] soundSources;

    private AudioSource musicAudiosource;


    private void Awake()
    {
        instance = this;
        musicAudiosource = GetComponent<AudioSource>();
        musicAudiosource.volume = musicVolume;
        musicAudiosource.loop = true;

        // 가용 가능한 soundSource 배열 생성
        soundSources = new SoundSource[Define.AVAILABLE_SOUNDSORUCE_COUNT];
    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip);
        GenerateAvailableSoundSource();
    }

    #region AudioManager 초기화

    private void GenerateAvailableSoundSource()
    {
        // 게임 내에서 사용할 SoundSource 오브젝트 생성
        for(int i=0; i<soundSources.Length; i++)
        {
            SoundSource soundSource = Instantiate(instance.soundSourcePrefab).GetComponent<SoundSource>();

            // 비활성화 된 상태로 초기화
            soundSource.gameObject.SetActive(false);

            soundSources[i] = soundSource;

        }
    }

    #endregion

    #region Audio Play

    public void ChangeBackGroundMusic(AudioClip clip)
    {
        // 배경음악 변경
        musicAudiosource.Stop();
        musicAudiosource.clip = clip;
        musicAudiosource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        // SoundEffect 효과음 틀기

        // 1. 가용 가능한 SoundSource 찾기
        SoundSource availableSoundSource = GetAvailableSoundSource();

        if(availableSoundSource == null)
        {
            Debug.Log("사용할 수 있는 SoundSource가 없습니다!");
            return;
        }

        // 효과음 재생
        availableSoundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);
    }

    public static SoundSource GetAvailableSoundSource()
    {
        for(int i=0; i< instance.soundSources.Length; i++)
        {
            // 비활성화 된(가용가능한) SoundSource를 찾아서 반환
            if(instance.soundSources[i].gameObject.activeSelf == false)
            {
                instance.soundSources[i].gameObject.SetActive(true);
                return instance.soundSources[i];
            }
        }

        return null;
    }

    #endregion

    #region Audio Setting

    public void SetBackGroundVolume(int value)
    {
        musicAudiosource.volume = value;
    }

    public void SetSoundEffectVolume(int value)
    {
        soundEffectVolume = value;
    }

    #endregion
}
