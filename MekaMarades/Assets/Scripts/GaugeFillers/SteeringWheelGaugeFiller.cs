using System;
using UnityEngine;

public class SteeringWheelGaugeFiller : GaugeFiller
{
    [SerializeField] private float m_needleSpeed;

    [NonSerialized] private float m_currentNeedlePosition;
    [NonSerialized] private float m_currentScoringZonePosition;


    [NonSerialized] private float m_timeSpentInScoringZone;


    public override float GetAdditionalFilling(float deltaTime)
    {
        return 0.0f;
    }
}
