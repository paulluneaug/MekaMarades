using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    
    [SerializeField] private GameObject m_startScene;
    [SerializeField] private GameObject m_endScene;

    void Start()
    {
        score.text = "Score : " + ScoreManager.Score;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
