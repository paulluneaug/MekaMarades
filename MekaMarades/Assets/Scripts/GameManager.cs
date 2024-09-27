using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtility.CustomAttributes;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_actionAsset;

    [Title("Global Decay Multiplier")]
    [SerializeField] private float m_startGlobalDecay = 0.5f;
    [SerializeField] private float m_startTimeBeforeDecay = 5.0f;

    [SerializeField] private float m_globalDecayIncreaseAmount = 0.1f;
    [SerializeField] private float m_globalDecayIncreaseTime = 10.0f;


    [NonSerialized] private Coroutine m_globalDecayCoroutine;

    void Awake()
    {
        m_actionAsset.Enable();
    }

    public void StartGlobalDecay()
    {
        m_globalDecayCoroutine = StartCoroutine(ManageGlobalDecay());
    }

    private void OnDestroy()
    {
        if (m_globalDecayCoroutine != null)
        {
            StopCoroutine(m_globalDecayCoroutine);
        }
    }


    private IEnumerator ManageGlobalDecay()
    {
        Gauge.GlobalDecayMultiplier = 0.0f;
        yield return new WaitForSeconds(m_startTimeBeforeDecay);
        Gauge.GlobalDecayMultiplier = m_startGlobalDecay;

        while (true)
        {
            yield return new WaitForSeconds(m_globalDecayIncreaseTime);
            Gauge.GlobalDecayMultiplier += m_globalDecayIncreaseAmount;
        }
    }
}
