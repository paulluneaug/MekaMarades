using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtility.Timer;

public class DepthSensorGaugeFiller : GaugeFiller
{
    [SerializeField] private float m_wallDefaultDistance;

    [SerializeField] private ArduinoConnectorManager m_arduinoManager;
    [SerializeField] private float m_addedGaugeValue;

    [SerializeField] private Timer m_timerBetweenShovels;

    [NonSerialized] private bool m_canShovel;

    private void Start()
    {
        m_canShovel = true;
        m_timerBetweenShovels.Start();
        m_timerBetweenShovels.OnTimerEnds += OnShovelTimerEnds;
    }

    private void Update()
    {
        m_timerBetweenShovels.Update(Time.deltaTime);
    }

    private void OnShovelTimerEnds()
    {
        m_canShovel = true;
    }

    public override float GetAdditionalFilling(float deltaTime)
    {
        if (CheckSensorDistance())
        {
            return m_addedGaugeValue;
        }
        return 0.0f;
    }

    private bool CheckSensorDistance()
    {
        Queue<byte> sensorDistances = m_arduinoManager.SensorDistances;

        if (!m_canShovel)
        {
            sensorDistances.Clear();
            return false;
        }

        byte readDistance;

        bool found = false;

        while (sensorDistances.TryDequeue(out readDistance))
        {
            if (readDistance >= m_wallDefaultDistance)
            {
                continue;
            }
            Debug.Log($"Positive : {readDistance}");
            found = true;
        }

        if (found)
        {
            m_timerBetweenShovels.Start();
            m_canShovel = false;
            return true;
        }
        return false;
    }
}
