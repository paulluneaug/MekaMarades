using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoConnectorManager : MonoBehaviour
{
    [SerializeField] private ArduinoConnector m_sensorConnector;

    [NonSerialized] private int m_sensorDistance = 0;

    public int SensorDistance => m_sensorDistance;

    void Start()
    {
        m_sensorConnector.Init();

        m_sensorConnector.OnMessageRecieved += OnSensorMessageRecieved;
    }

    private void OnDestroy()
    {
        m_sensorConnector.Close();
    }

    private void OnSensorMessageRecieved(byte[] buffer, int recievedBytesCount)
    {
        m_sensorDistance = buffer[0];
        //Debug.Log($"Ditance = {m_sensorDistance}");
    }

    [ContextMenu("PING")]
    private void Ping()
    {
    }
}
