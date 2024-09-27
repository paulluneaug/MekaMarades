using System;
using System.Linq;
using UnityEngine;

public class GaugeManager : MonoBehaviour
{
    [SerializeField] private Gauge[] m_gauges;
    [SerializeField, Range(1,4)] private int m_minBrokenGauge;


    [SerializeField] private GameObject m_mainScene;
    [SerializeField] private GameObject m_endScene;
    private void Update()
    {
        if(m_gauges.Count(x => x.IsBroken()) >= m_minBrokenGauge)
            EndScene();
    }

    private void EndScene()
    {
        m_endScene.SetActive(true);
        m_mainScene.SetActive(false);
    }
}
