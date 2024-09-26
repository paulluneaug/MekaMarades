using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtility.CustomAttributes;
using UnityUtility.Utils;

using Random = UnityEngine.Random;

public class SteeringWheelGame : MonoBehaviour
{
    [SerializeField] private InputActionReference m_wheelAction;

    [Title("Needle Parameters")]
    [SerializeField, Range(0, 5)] private float m_needleSpeed;
    [SerializeField, Range(0, 100)] private float m_needleWidth;

    [Title("ScoringZone Parameters")]
    [SerializeField, Range(0, 5)] private float m_scoringZoneSpeed;
    [SerializeField, Range(0, 100)] private float m_scoringZoneWidth;
    [SerializeField, Range(0, 1)] private float m_scoringZoneTargetTolerance;

    [Title("Transforms")]
    [SerializeField] private RectTransform m_needle;
    [SerializeField] private RectTransform m_scoringZone;
    [SerializeField] private RectTransform m_backgroundZone;


    [NonSerialized] private float m_needlePosition;
    [NonSerialized] private float m_scoringZonePosition;
    [NonSerialized] private float m_backgroundZoneWidth;

    [NonSerialized] private float m_scoringZoneTarget;

    //[NonSerialized] private Vector2 m_scoringZoneRange;



    // Start is called before the first frame update
    private void Start()
    {
        m_needlePosition = 0.5f;
        m_scoringZonePosition = 0.5f;


        m_backgroundZoneWidth = m_backgroundZone.rect.width;
        m_needle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_backgroundZoneWidth * m_needleWidth / 100);
        m_scoringZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_backgroundZoneWidth * m_scoringZoneWidth / 100);

        m_scoringZoneTarget = GetNewScoringZoneTarget();

        MoveScoringZone();

        //m_scoringZoneRange = new Vector2(m_scoringZoneWidth / 2, 1 - (m_scoringZoneWidth / 2));
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCursor(m_wheelAction.action.ReadValue<float>());
        MoveScoringZone();
    }

    private void MoveScoringZone()
    {
        if (m_scoringZonePosition.Approximately(m_scoringZoneTarget, m_scoringZoneTargetTolerance))
        {
            m_scoringZoneTarget = GetNewScoringZoneTarget();
        }

        float distToTarget = m_scoringZoneTarget - m_scoringZonePosition;
        m_scoringZonePosition += Mathf.Min(Mathf.Abs(distToTarget), m_scoringZoneSpeed * Time.deltaTime) * Mathf.Sign(distToTarget);


        //m_scoringZonePosition += Random.Range(-1f, 1f) * Time.deltaTime * m_scoringZoneSpeed;
        //m_scoringZonePosition = Mathf.Clamp01(m_scoringZonePosition);
        m_scoringZone.anchoredPosition = new Vector3(GetInGamePosition(m_scoringZone, m_scoringZonePosition), 0, 0);
    }

    private float GetNewScoringZoneTarget()
    {
        return Random.value;
    }

    private void MoveCursor(float speed)
    {
        m_needlePosition += m_needleSpeed * speed * 0.01f;
        m_needlePosition = Mathf.Clamp01(m_needlePosition);
        m_needle.anchoredPosition = new Vector3(GetInGamePosition(m_needle, m_needlePosition), 0, 0);
    }

    private float GetInGamePosition(RectTransform rectTransform, float virtualPosition)
    {
        float rectHalfWidth = rectTransform.rect.width / 2;
        return Mathf.Lerp(rectHalfWidth, m_backgroundZoneWidth - rectHalfWidth, virtualPosition);
    }

    public bool IsNeedleInScoringBounds()
    {
        return Mathf.Abs(m_needlePosition - m_scoringZonePosition) < Mathf.Abs(m_scoringZoneWidth - m_needleWidth) / 200;
    }
}
