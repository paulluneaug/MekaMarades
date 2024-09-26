using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtility.Timer;

public class PumpGaugeFiller : GaugeFiller
{
    [SerializeField] private InputActionReference m_pumpAction;
    [SerializeField] private Timer m_timerBetweenPumps;
    [SerializeField] private float m_gaugeAddedValue;

    [NonSerialized] private bool m_canPump;
    [NonSerialized] private bool m_pumpedSinceLastCheck;


    private void Start()
    {
        m_canPump = true;
        m_pumpAction.action.performed += OnPumpActionPerformed;

        m_timerBetweenPumps.OnTimerEnds += OnPumpTimerEnds;
    }
    
    private void Update()
    {
        m_timerBetweenPumps.Update(Time.deltaTime);
    }

    private void OnPumpTimerEnds()
    {
        m_canPump = true;
    }

    private void OnPumpActionPerformed(InputAction.CallbackContext context)
    {
        if (!m_canPump) 
        {
            return;
        }
        m_canPump = false;
        m_pumpedSinceLastCheck = true;
        m_timerBetweenPumps.Reset();
        m_timerBetweenPumps.Start();
    }

    public override float GetAdditionalFilling(float deltaTime)
    {
        if (m_pumpedSinceLastCheck)
        {
            m_pumpedSinceLastCheck = false;
            return m_gaugeAddedValue;
        }
        return 0.0f;
    }
}
