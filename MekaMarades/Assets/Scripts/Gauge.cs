using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public float CurrentFilling => m_currentFilling;

    public static float GlobalDecayMultiplier = 0.5f;

    [SerializeField] private float m_decayRate;
    [SerializeField] private GaugeFiller m_filler;

    [SerializeField] private Image m_slider;

    [NonSerialized] private float m_currentFilling;
    [NonSerialized] private bool m_isBroken = false;

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
        if(IsBroken())
            return;
        
        float deltaTime = Time.deltaTime;
        float fillingDelta = m_filler.GetAdditionalFilling(deltaTime) - (deltaTime * m_decayRate * GlobalDecayMultiplier);

        m_currentFilling = Mathf.Clamp01(m_currentFilling + fillingDelta);
        
        if(m_currentFilling >= 1)
            m_onFilled.Invoke();
        else if (m_currentFilling <= m_alarmRange)
        {
            if (m_currentFilling <= 0)
            {
                m_isBroken = true;
                UpdateSlider();
                return;
            }
            
            m_onAlarm.Invoke();
        }

        UpdateSlider();
    }

    private void UpdateSlider()
    {
        m_slider.fillAmount = m_currentFilling;
    }

    public bool IsBroken()
    {
        return m_isBroken;
    }
}
