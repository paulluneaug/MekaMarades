using System;
using UnityEngine;
using UnityUtility.Utils;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] m_mainAudioSources;
    [SerializeField] private AudioSource[] m_endAudioSources;
    [SerializeField] private AudioSource m_alarmAudioSources;
    [SerializeField] private AudioSource m_successAudioSources;
    private void Start()
    {
        m_mainAudioSources.ForEach(x => x.Play());
    }

    public void PlaySucessSound()
    {
        if(!m_successAudioSources.isPlaying)
            m_successAudioSources.Play();
    }

    public void PlayAlarmSound()
    {
        if(!m_alarmAudioSources.isPlaying)
            m_alarmAudioSources.Play();
    }

    public void EndSound()
    {
        m_mainAudioSources.ForEach(x => x.Stop());
        m_endAudioSources.ForEach(x => x.Play());
    }
}
