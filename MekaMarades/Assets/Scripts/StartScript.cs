using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityUtility.CustomAttributes;

public class StartScript : MonoBehaviour
{
    [SerializeField] private Image[] m_stars;
    [SerializeField] private InputActionReference[] m_actions;
    [NonSerialized] private bool[] m_starsActive = { false, false, false, false };
    [SerializeField] private GameObject m_mainScene;
    [SerializeField] private GameObject m_startScene;

    [Header("other shit")]
    [SerializeField] private float m_wallDefaultDistance;
    [SerializeField] private ArduinoConnectorManager m_arduinoManager;

    [Title("Global Decay Multiplier")]
    [SerializeField] private float m_startGlobalDecay = 0.5f;
    [SerializeField] private float m_startTimeBeforeDecay = 5.0f;

    [SerializeField] private float m_globalDecayIncreaseAmount = 0.1f;
    [SerializeField] private float m_globalDecayIncreaseTime = 10.0f;

    [NonSerialized] private Coroutine m_globalDecayCoroutine;


    private void Update()
    {
        if (CheckSensorDistance())
            ModuleActivated(0);
        for (int loop = 0; loop < m_actions.Length; loop++)
        {
            InputActionReference action = m_actions[loop];
            if (action.action.ReadValue<float>() != 0)
                ModuleActivated(loop + 1);
        }

        foreach (bool active in m_starsActive)
        {
            if (!active)
                return;
        }
        StartGame();
    }

    private void OnDestroy()
    {
        if (m_globalDecayCoroutine != null)
        {
            StopCoroutine(m_globalDecayCoroutine);
        }
    }

    private void ModuleActivated(int starNb)
    {
        m_stars[starNb].color = Color.white;
        m_starsActive[starNb] = true;
    }

    private void StartGame()
    {
        m_mainScene.SetActive(true);
        m_startScene.SetActive(false);

        m_globalDecayCoroutine = StartCoroutine(ManageGlobalDecay());
    }

    private bool CheckSensorDistance()
    {
        Queue<byte> sensorDistances = m_arduinoManager.SensorDistances;

        byte readDistance;

        bool found = false;

        while (sensorDistances.TryDequeue(out readDistance))
        {
            if (readDistance >= m_wallDefaultDistance)
            {
                continue;
            }
            found = true;
        }

        if (found)
        {
            return true;
        }
        return false;
    }

    private IEnumerator ManageGlobalDecay()
    {
        Gauge.GlobalDecayMultiplier = 0.0f;
        yield return new WaitForSeconds(m_startTimeBeforeDecay);
        Gauge.GlobalDecayMultiplier = m_startGlobalDecay;

        while (true)
        {
            yield return new WaitForSeconds(m_globalDecayIncreaseTime);
            Gauge.GlobalDecayMultiplier = m_globalDecayIncreaseAmount;
        }
    }
}
