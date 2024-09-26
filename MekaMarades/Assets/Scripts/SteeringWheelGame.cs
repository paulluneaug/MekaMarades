using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteeringWheelGame : MonoBehaviour
{
    [SerializeField] private InputActionReference m_wheelAction;

    [SerializeField] private float m_needleSpeed;

    [SerializeField] private RectTransform m_needle;
    [SerializeField] private RectTransform m_scoringZone;

    [SerializeField] private float m_scoringZoneWidth;

    [NonSerialized] private float m_needlePosition;
    [NonSerialized] private float m_scoringZonePosition;

    [NonSerialized] private Vector2 m_scoringZoneRange;



    // Start is called before the first frame update
    private void Start()
    {
        m_needlePosition = 0.5f;
        m_scoringZonePosition = 0.5f;

        m_scoringZoneRange = new Vector2(m_scoringZoneWidth / 2, 1 - (m_scoringZoneWidth / 2));
    }

    // Update is called once per frame
    private void Update()
    {
        m_wheelAction.action.ReadValue<float>();
    }

    private void MoveScoringZone()
    {

    }

    private void UpdateUI()
    {

    }
}
