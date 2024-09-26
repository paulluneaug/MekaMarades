using System;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public float CurrentFilling => m_currentFilling;

    [SerializeField] private float m_decayRate;
    [SerializeField] private GaugeFiller m_filler;

    [SerializeField] private Image m_slider;

    [NonSerialized] private float m_currentFilling;

    private void Start()
    {
        m_currentFilling = 1.0f;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float fillingDelta = m_filler.GetAdditionalFilling(deltaTime) - (deltaTime * m_decayRate);

        m_currentFilling = Mathf.Clamp01(m_currentFilling + fillingDelta);

        UpdateSlider();
    }

    private void UpdateSlider()
    {
        m_slider.fillAmount = m_currentFilling;
    } 
}
