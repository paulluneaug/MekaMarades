using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoConnectorManager : MonoBehaviour
{
    [SerializeField] private ArduinoConnector m_sensorConnector;

    void Start()
    {
        m_sensorConnector.Init();

        m_sensorConnector.OnMessageRecieved += OnSensorMessageRecieved;
    }

    private void OnDestroy()
    {
        m_sensorConnector.Close();
    }

    private void OnSensorMessageRecieved(string obj)
    {
    }

    [ContextMenu("PING")]
    private void Ping()
    {
        m_sensorConnector.Send("PING");
    }
}
