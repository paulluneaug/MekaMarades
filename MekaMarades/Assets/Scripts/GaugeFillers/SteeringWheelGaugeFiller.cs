using System;
using UnityEngine;

public class SteeringWheelGaugeFiller : GaugeFiller
{
    [SerializeField] private float m_fillingSpeed = 10f;
    [SerializeField] private SteeringWheelGame m_steeringWheelGame;

    [NonSerialized] private float m_currentNeedlePosition;
    [NonSerialized] private float m_currentScoringZonePosition;


    [NonSerialized] private float m_timeSpentInScoringZone;


    public override float GetAdditionalFilling(float deltaTime)
    {
        return m_steeringWheelGame.IsNeedleInScoringBounds() ? m_fillingSpeed*0.001f : 0f;
    }
}
