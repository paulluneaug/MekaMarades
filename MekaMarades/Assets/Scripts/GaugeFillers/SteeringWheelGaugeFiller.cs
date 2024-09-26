using System;
using UnityEngine;

public class SteeringWheelGaugeFiller : GaugeFiller
{
    [SerializeField] private SteeringWheelGame m_steeringWheelGame;
    [SerializeField] private float m_fillingSpeed = 10f;
    [SerializeField] private float m_timeBeforeScoring = 0.5f;

    [NonSerialized] private float m_timeSpentInScoringZone;

    private void Start()
    {
        m_timeSpentInScoringZone = 0.0f;
    }


    public override float GetAdditionalFilling(float deltaTime)
    {
        if (!m_steeringWheelGame.IsNeedleInScoringBounds())
        {
            m_timeSpentInScoringZone = 0.0f;
            return 0.0f;
        }

        m_timeSpentInScoringZone += deltaTime;

        if (m_timeSpentInScoringZone < m_timeBeforeScoring)
        {
            return 0.0f;
        }

        return m_fillingSpeed * 0.001f;
    }
}
