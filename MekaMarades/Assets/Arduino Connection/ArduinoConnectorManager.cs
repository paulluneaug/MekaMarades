using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoConnectorManager : MonoBehaviour
{
    private const int QUEUE_CAPACITY = 100;

    private const byte ULTRASONIC_HEADER = 10;
    private const byte DMX_COMMAND_HEADER = 20;
    private const byte ERROR_HEADER = 245;

    public Queue<byte> SensorDistances => m_sensorDistances;

    [SerializeField] private ArduinoConnector m_sensorConnector;

    [NonSerialized] private Queue<byte> m_recievedDatas;

    [NonSerialized] private Queue<byte> m_sensorDistances;

    private void Start()
    {
        m_recievedDatas = new Queue<byte>(QUEUE_CAPACITY);
        m_sensorDistances = new Queue<byte>(QUEUE_CAPACITY);

        m_sensorConnector.Init();

        m_sensorConnector.OnMessageRecieved += OnArduinoMessageRecieved;
    }

    private void OnDestroy()
    {
        m_sensorConnector.Close();
    }

    public void SendDMXCommand(byte channel, byte value) 
    {
        int indexInBuffer = 0;
        Span<byte> buffer = stackalloc byte[3];

        buffer[indexInBuffer++] = DMX_COMMAND_HEADER;
        buffer[indexInBuffer++] = channel;
        buffer[indexInBuffer++] = value;

        m_sensorConnector.Send(buffer);
    }

    private void OnArduinoMessageRecieved(byte[] buffer, int recievedBytesCount)
    {
        for (int i = 0; i < recievedBytesCount; i++)
        {
            m_recievedDatas.Enqueue(buffer[i]);
        }


        bool recievedEnoughDatas = true;
        while (m_recievedDatas.Count > 0 && recievedEnoughDatas)
        {
            byte queueHead = m_recievedDatas.Peek();
            switch (queueHead)
            {
                case ULTRASONIC_HEADER:
                    recievedEnoughDatas &= TryProcessUltrasonicDatas(m_recievedDatas);
                    break;

                case ERROR_HEADER:
                    recievedEnoughDatas &= TryProcessErrorDatas(m_recievedDatas);
                    break;

                default: // The header is discarded if unknown
                    Debug.LogError($"Unknown Header ({queueHead}) Next commands might not be working properly");
                    m_recievedDatas.Dequeue();
                    break;
            }
        }
        //Debug.Log($"Ditance = {m_sensorDistance}");
    }

    private bool TryProcessUltrasonicDatas(Queue<byte> recievedDatas)
    {
        if (recievedDatas.Count < 2)
        {
            return false; // Should wait for more datas to arrive
        }

        recievedDatas.Dequeue(); // Dequeue the header

        byte newDist = recievedDatas.Dequeue();

        while (m_sensorDistances.Count >= QUEUE_CAPACITY)
        {
            m_sensorDistances.Dequeue();
        }
        m_sensorDistances.Enqueue(newDist);
        return true; // The command was processed and removed from the queue
    }

    private bool TryProcessErrorDatas(Queue<byte> recievedDatas)
    {
        if (recievedDatas.Count < 2)
        {
            return false; // Should wait for more datas to arrive
        }

        recievedDatas.Dequeue(); // Dequeue the header

        byte errorCode = recievedDatas.Dequeue();

        Debug.LogError($"ArduinoError recieved : {errorCode}");
        return true; // The command was processed and removed from the queue
    }

    private IEnumerator Flicker(byte channel, float duration, bool intermediateValues)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            byte newChannelValue = (byte)(intermediateValues ? 
                UnityEngine.Random.Range(0, 255) : 
                UnityEngine.Random.Range(0, 2) * 255);

            SendDMXCommand(channel, newChannelValue);


            timer += Time.deltaTime;
            yield return null;
        }
    }

    [ContextMenu("Flicker")]
    private void StartFlicker()
    {
        StartCoroutine(Flicker(DMXChannelsGlossary.DIMMER_1_CHANNEL, 2.0f, true));
    }

    [ContextMenu("Command")]
    private void SendCommand()
    {
        SendDMXCommand(DMXChannelsGlossary.DIMMER_3_CHANNEL, 255);
    }

}
