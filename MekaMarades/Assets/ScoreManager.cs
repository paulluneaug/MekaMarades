using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int m_scoreToAddEachStep;
    [SerializeField] private float m_timeBeforeIncreaseScore;
    [SerializeField] private TMP_Text m_scoreText;

    private int m_score;
    private float m_timer;

    private void Awake()
    {
        m_score = 0;
        m_timer = 0;
    }

    private void Update()
    {
        if (m_timer < m_timeBeforeIncreaseScore)
        {
            m_timer += Time.deltaTime;
            return;
        }

        m_score += m_scoreToAddEachStep;

        m_scoreText.text = m_score.ToString();
        m_timer = 0;
    }


}
