using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public float CurrentFilling => m_currentFilling;

    [SerializeField] private float m_decayRate;
    [SerializeField] private GaugeFiller m_filler;

    [SerializeField] private Image m_slider;

    [NonSerialized] private float m_currentFilling;

    [Header("Sounds")]
    [SerializeField] private float m_alarmRange = .1f;
    [SerializeField] private UnityEvent m_onAlarm = new();
    [SerializeField] private UnityEvent m_onFilled = new();

    private void Start()
    {
        m_currentFilling = 1.0f;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float fillingDelta = m_filler.GetAdditionalFilling(deltaTime) - (deltaTime * m_decayRate);

        m_currentFilling = Mathf.Clamp01(m_currentFilling + fillingDelta);
        
        if(m_currentFilling >= 1)
            m_onFilled.Invoke();
        else if(m_currentFilling <= m_alarmRange)
            m_onAlarm.Invoke();

        UpdateSlider();
    }

    private void UpdateSlider()
    {
        m_slider.fillAmount = m_currentFilling;
    } 
}
