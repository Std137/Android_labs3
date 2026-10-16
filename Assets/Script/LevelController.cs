using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class LevelController : MonoBehaviour
{
    [Header("Панели")]
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public GameObject PausePanel;
    public GameObject ExitLetter;
    public GameObject ScorePanel;
    public GameObject TimePanel;
    public GameObject MainPanel;
    public GameObject GameName;
    public GameObject Player;
    public GameObject touchJoystick;


    
    [Header("Контроллеры")]
    public ScoreManager ScoreManager; //Счетчик поинтов
    public GameTimer GameTimer;       //Счетчик времни
    public MessageShow MsgShow;        //Действия
    public InputSystemUIInputModule _module;

    [Header("Позиция для меню")]
    public Vector3 MenulookAtPosition;

    [Header("Стартовая позиция")]
    public Vector3 StarttargetPosition;
    public Vector3 StartlookAtPosition;

    private LelelFieldGenerate lg = new LelelFieldGenerate();
    private bool isLevelActive;
    private bool isPaused = false;
    private TouchPlayerControl PlayerMoveController;
    private Joystick joystick;
    private InputAction _cancel;

    void Awake() => PlayerMoveController = Player.GetComponent<TouchPlayerControl>();

    public void LevelGenerator()
    {
        Quaternion rotateWall = Quaternion.Euler(0f, 90f, 0f);
        Quaternion norotateWall = Quaternion.Euler(0f, 0f, 0f);
        GameObject cloneWall;
        var originalWall = GameObject.FindGameObjectWithTag("WallTemplate");
        var originalLetter = GameObject.FindGameObjectWithTag("LetterTemplate");
        var levelObj = GameObject.FindGameObjectWithTag("CopyObj").transform;
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                if (lg.GetRight(x, y))
                {
                    cloneWall = Object.Instantiate(originalWall, new Vector3((-30f + 10 * y), 10f, (35f - 10 * x)), norotateWall);
                    cloneWall.transform.SetParent(levelObj, worldPositionStays: true);
                }
                if (lg.GetDown(x, y))
                {

                    cloneWall = Object.Instantiate(originalWall, new Vector3((-35f + 10 * y), 10f, (30f - 10 * x)), rotateWall);
                    cloneWall.transform.SetParent(levelObj, worldPositionStays: true);
                }
                var txtField = lg.GetLetter(x, y);
                if ((txtField != " "))
                {
                    var cloneLetter = Object.Instantiate(originalLetter, new Vector3((-35f + 10 * y), 3f, (35f - 10 * x)), norotateWall);
                    cloneLetter.transform.SetParent(levelObj, worldPositionStays: true);
                    var tmph = cloneLetter.GetComponent<TMP_Text>();
                    if (tmph != null) tmph.text = txtField;
                }
            }
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        GameTimer.TimerStop();
        ScoreManager.ScoreReset();
        MsgShow.MsgStop();
        ScoreManager.onScoreExpired.RemoveListener(OnTimerExpried);
        GameTimer.onTimeExpired.RemoveListener(OnScoreExpried);
        ScorePanel.SetActive(false);
        TimePanel.SetActive(false);
#if UNITY_ANDROID
        touchJoystick.SetActive(false);
#endif
        ExitLetter.SetActive(false);
    }


    public void StartLevel() 
    {
        if (isLevelActive) return;
#if UNITY_ANDROID
        touchJoystick.SetActive(true);
        joystick = touchJoystick?.GetComponent<Joystick>();
        joystick.ResetJoystick();
#endif
        GameName.SetActive(false);
        PlayerMoveController.stopMove();
        PlayerMoveController.PlayerTeleport(StarttargetPosition, StartlookAtPosition);
        LevelGenerator();
        ScorePanel.SetActive(true);
        ScoreManager.ScoreReset();
        TimePanel.SetActive(true);
        ScoreManager.onScoreExpired.AddListener(OnScoreExpried);
        ScoreManager.onLastLetter.AddListener(onLastLetter);
        GameTimer.onTimeExpired.AddListener(OnTimerExpried);
        GameTimer.TimerStart();
        isLevelActive = true;
        PlayerMoveController.startMove();
        Time.timeScale = 1f;
        MsgShow.MsgShow("Let's Go!");
    }
    public void ExitLevel()
    {
        var levelObj = GameObject.FindGameObjectWithTag("CopyObj").transform;
        isLevelActive = false;
        isPaused = false;
        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
        PausePanel.SetActive(false);
        for (int i = levelObj.childCount - 1; i >= 0; i--)
        {
            if (levelObj.GetChild(i).gameObject != null) Destroy(levelObj.GetChild(i).gameObject);
        }
        PlayerMoveController.stopMove();
        PlayerMoveController.PlayerTeleport(StarttargetPosition, MenulookAtPosition);
        GameName.SetActive(true);
        MainPanel.SetActive(true);
    }


    public void OnCancelClick()
    {
        if (!isLevelActive) return;
        if (!isPaused)
        {
            onClickPause();
            isPaused = true;
        }
        else
        {
            onClickResume();
            isPaused = false;
        }
    }

    public void onClickPause()
    {   
        Time.timeScale = 0f;
        GameTimer.PauseTimer();
        MsgShow.MsgStop();
        PausePanel.SetActive(true);
    }

    public void onClickResume()
    {
        Time.timeScale = 1f;
        GameTimer.PauseTimer();
        PausePanel.SetActive(false);
    }

    public void onOkExit()
    {
        StopGame();
        ExitLevel();
    }

    //Вышло время
    private void OnTimerExpried()
    {
        StopGame();
        GameOverPanel.SetActive(true);
    }
    
    //Все собрано
    private void OnScoreExpried()
    {
        if (GameTimer.IsTimeNotExpired())
        {
            ExitLetter.SetActive(true);
            MsgShow.MsgShow("Exit activ");
        }
    }

    private void onLastLetter()
    {
            MsgShow.MsgShow("Last Letter");
    }
    
    public void OnWin()
    {
        GameTimer.PauseTimer();
        if (GameTimer.IsTimeNotExpired())
        {
            StopGame();
            WinPanel.SetActive(true);
        }
    }

    void OnEnable() { _cancel = _module.cancel; _cancel.performed += OnCancelPerformed; }
    void OnDisable() { _cancel.performed -= OnCancelPerformed; }
    
    private void OnCancelPerformed(InputAction.CallbackContext ctx)
    {
        OnCancelClick();
    }
}
