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
    [SerializeField] private GameObject m_mainScene;
    [SerializeField] private GameObject m_startScene;

    [SerializeField] private GameManager m_gameManager;

    [Header("other shit")]
    [SerializeField] private float m_wallDefaultDistance;
    [SerializeField] private ArduinoConnectorManager m_arduinoManager;


    [NonSerialized] private bool[] m_starsActive = { false, false, false, false };


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

    private void ModuleActivated(int starNb)
    {
        m_stars[starNb].color = Color.white;
        m_starsActive[starNb] = true;
    }

    private void StartGame()
    {
        m_mainScene.SetActive(true);
        m_startScene.SetActive(false);

        m_gameManager.StartGlobalDecay();
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
}
