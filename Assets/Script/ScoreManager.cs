using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager: MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;

    [Header("События")]
    public UnityEvent onScoreExpired;

    public UnityEvent onLastLetter;

    private int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void ScoreUP()
    {
        score++;
        scoreText.text = score.ToString();
        if (score == 12) onLastLetter?.Invoke();
        if (score == 13) onScoreExpired?.Invoke();
    }

    public void ScoreReset()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    public int CurrentScore() => score;
}
