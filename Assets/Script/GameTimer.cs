using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance;

    [Header("Настройки")]
    [SerializeField] public TextMeshProUGUI timerText;
    

    [Header("События")]
    public UnityEvent onTimeExpired;
    private float totalTime = 80f;
    private float currentTime;
    private bool isRunning;

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

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            onTimeExpired?.Invoke();
        }
        else
        {
            timerText.text = currentTime.ToString("0.0");
        }
    }

    public void TimerStart()
    {
        currentTime = totalTime;
        isRunning = true;
    }

    public void PauseTimer() => isRunning = !isRunning;
    
    public bool IsTimeNotExpired() => currentTime > 0f;

    public void TimerStop()
    {
        currentTime = totalTime;
        isRunning = false;
    }
}