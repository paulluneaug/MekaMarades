using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int m_scoreToAddEachStep;
    [SerializeField] private float m_timeBeforeIncreaseScore;

    public static int Score {get; private set;}
    private float m_timer;

    private void Awake()
    {
        Score = 0;
        m_timer = 0;
    }

    private void Update()
    {
        if (m_timer < m_timeBeforeIncreaseScore)
        {
            m_timer += Time.deltaTime;
            return;
        }

        Score += m_scoreToAddEachStep;
        m_timer = 0;
    }


}
