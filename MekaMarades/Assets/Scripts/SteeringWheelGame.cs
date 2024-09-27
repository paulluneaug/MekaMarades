using System;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = Unity.Mathematics.Random;

public class SteeringWheelGame : MonoBehaviour
{
    [SerializeField] private InputActionReference m_wheelAction;
    
    [Header("Needle Parameters")]
    [SerializeField, Range(0,1)] private float m_needleSpeed;
    [SerializeField, Range(0,100)] private float m_needleWidth;
    [Header("ScoringZone Parameters")]
    [SerializeField, Range(0,1)] private float m_scoringZoneSpeed;
    [SerializeField, Range(0,100)] private float m_scoringZoneWidth;
    [SerializeField, Range(0,5)] private float m_minNextScoringZoneTime;
    [SerializeField, Range(0,5)] private float m_maxNextScoringZoneTime;

    [Header("Transforms")]
    [SerializeField] private RectTransform m_needle;
    [SerializeField] private RectTransform m_scoringZone;
    [SerializeField] private RectTransform m_backgroundZone;


    [NonSerialized] private float m_needlePosition;
    [NonSerialized] private float m_scoringZonePosition;
    [NonSerialized] private float m_backgroundZoneWidth;
    
    [NonSerialized] private float m_scoringZoneNextPosition;
    [NonSerialized] private Timer m_scoringZoneTimer = new();
    
    
    [NonSerialized] private Random m_random;

    //[NonSerialized] private Vector2 m_scoringZoneRange;



    // Start is called before the first frame update
    private void Start()
    {
        // Random
        m_random.InitState();
        
        // Default positions
        m_needlePosition = 0.5f;
        m_scoringZonePosition = 0.5f;

        // Scoring Timer
        m_scoringZoneTimer.AutoReset = false;
        m_scoringZoneTimer.Elapsed += FindNextScoringPosition;
        m_scoringZoneTimer.Start();
        
        // Set Timer limits
        m_minNextScoringZoneTime *= 1000;
        m_maxNextScoringZoneTime *= 1000;

        // Set width
        m_backgroundZoneWidth = m_backgroundZone.rect.width;
        m_needle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_backgroundZoneWidth*m_needleWidth/100);
        m_scoringZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_backgroundZoneWidth*m_scoringZoneWidth/100);

        
        MoveScoringZone();
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCursor(m_wheelAction.action.ReadValue<float>());
        MoveScoringZone();
    }

    private void MoveScoringZone()
    {
        if (Mathf.Abs(m_scoringZonePosition - m_scoringZoneNextPosition) < 0.01f)
        {
            return;
        }

        m_scoringZonePosition = Mathf.Lerp(m_scoringZonePosition, m_scoringZoneNextPosition, m_scoringZoneSpeed/10);
        m_scoringZone.anchoredPosition = new Vector3(GetInGamePosition(m_scoringZone, m_scoringZonePosition), 0, 0);
    }

    private void FindNextScoringPosition(object _sender, ElapsedEventArgs _elapsedEventArgs)
    {
        m_scoringZoneTimer.Stop();
        m_scoringZoneNextPosition = m_random.NextFloat(0f, 1f);
        m_scoringZoneTimer.Interval = m_random.NextFloat(m_minNextScoringZoneTime, m_maxNextScoringZoneTime);
        m_scoringZoneTimer.Start();
    }

    private void MoveCursor(float speed)
    {
        m_needlePosition += m_needleSpeed*speed*0.01f;
        m_needlePosition = Mathf.Clamp01(m_needlePosition);
        m_needle.anchoredPosition = new Vector3(GetInGamePosition(m_needle, m_needlePosition), 0, 0);
    }

    private float GetInGamePosition(RectTransform rectTransform, float virtualPosition)
    {
        float rectHalfWidth = rectTransform.rect.width/2;
        return Mathf.Lerp(rectHalfWidth, m_backgroundZoneWidth-rectHalfWidth, virtualPosition);
    }

    public bool IsNeedleInScoringBounds()
    {
        return Mathf.Abs(m_needlePosition - m_scoringZonePosition) < Mathf.Abs(m_scoringZoneWidth - m_needleWidth)/200;
    }
}
