using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ArduinoConnector
{
    public event Action<string> OnMessageRecieved;

    [SerializeField] private string m_port;
    [SerializeField] private int m_baud;

    private SerialPort m_serialPort;

    public void Init()
    {
        foreach (string s in SerialPort.GetPortNames())
        {
            Debug.Log($"   {s}");
        }

        m_serialPort = new SerialPort(m_port, m_baud);
        m_serialPort.ReadTimeout = 50;

        m_serialPort.RtsEnable = true;
        m_serialPort.DtrEnable = true;

        Debug.Log(m_serialPort.IsOpen);
        m_serialPort.Open();

        Task.Factory.StartNew(AwaitDatas);
    }

    public void Close()
    {
        m_serialPort.Close();
    }

    public void Send(string message)
    {
        byte[] buffer = new byte[10];
        m_serialPort.BaseStream.Write(buffer, 0, 10);
        //m_serialPort.WriteLine(message);
        m_serialPort.BaseStream.Flush();
        Debug.Log($"Sending {message}");
    }

    private Task AwaitDatas()
    {
        while(true)
        {
            string dataString = null;
            try
            {
                //dataString = m_serialPort.ReadExisting();
                dataString = m_serialPort.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (!string.IsNullOrEmpty(dataString))
            {
                Debug.Log($"Recieved : {dataString}");
                OnMessageRecieved.Invoke(dataString);
            }
            Task.Delay(10);
        }
    }
}

