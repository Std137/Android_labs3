using UnityEngine;


public class GameController : MonoBehaviour
{
    [Header("Панели")]
    public GameObject mainMenuPanels;
    public GameObject exitPanel;
    public GameObject aboutPanel;
    public LevelController LevelController;

    private float _nextCheck;      //Таймер спрятать панель навигации

    public void Start()
    {
        mainMenuPanels.SetActive(true);
        Time.timeScale = 0f;
        Screen.fullScreen = true;
        GameController.HideSystemBars();
    }

    public void onClickBtnExitYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void onClickBtnExitNo()
    {
        mainMenuPanels.SetActive(true);
        exitPanel.SetActive(false);
    }

    public void onClickBtnExitGame()
    {
        exitPanel.SetActive(true);
        mainMenuPanels.SetActive(false);
    }

    public void onClickBtnAboutGame()
    {
        aboutPanel.SetActive(true);
        mainMenuPanels.SetActive(false);
    }

    public void onClickBtnAboutOk()
    {
        mainMenuPanels.SetActive(true);
        aboutPanel.SetActive(false);
    }

    public void onClickBtnStartGame()
    {
        mainMenuPanels.SetActive(false);
        LevelController.StartLevel();
        
    }


    void OnApplicationFocus(bool hasFocus)
    {
        GameController.HideSystemBars();
    }

    public static void HideSystemBars()
    {
 #if UNITY_ANDROID && !UNITY_EDITOR
        int apiLevel = new AndroidJavaClass("android.os.Build$VERSION")
            .GetStatic<int>("SDK_INT");

        if (apiLevel >= 30)
        {
            // Android 11+
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var window = activity.Call<AndroidJavaObject>("getWindow"))
            using (var decorView = window.Call<AndroidJavaObject>("getDecorView"))
            {
                using (var controller = decorView.Call<AndroidJavaObject>("getWindowInsetsController"))
                {
                    if (controller == null) return;

                    controller.Call("setSystemBarsBehavior", 2);
                    controller.Call("hide", 7);
                }
            }
        }
        else
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var window = activity.Call<AndroidJavaObject>("getWindow"))
            using (var decorView = window.Call<AndroidJavaObject>("getDecorView"))
            {
                decorView.Call("setSystemUiVisibility",
                    0x00000002 |  
                    0x00000004 |  
                    0x00000008 |  
                    0x00001000 |  
                    0x00000100 |  
                    0x00000200 |  
                    0x00000100);  
            }
        }
#endif
    }
}