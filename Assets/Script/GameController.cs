using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


public class GameController : MonoBehaviour
{
    [Header("Панели")]
    public GameObject mainMenuPanels;
    public GameObject exitPanel;
    public GameObject aboutPanel;
    public GameObject originaWall;
    public GameObject touchJoystick;

    private bool isMobile = true;

    
   
    
    private void CopyWall(int x, int y, bool rotate)
    {
        var localScaleX = (rotate) ? 10 : 1;
        var localScaleZ = (rotate) ? 1 : 10;
        var localPositionX = x * 10.0f;
        var localPositionY = y * 10.0f;
        GameObject wall = Instantiate(originaWall);
        wall.transform.localScale = new Vector3(localScaleX, 20, localScaleZ);
        wall.transform.position = new Vector3(localPositionX, 10.0f, localPositionY);
    }

    public void Start()
    {
        mainMenuPanels.SetActive(true);
        Time.timeScale = 0f;
        if (Application.platform == RuntimePlatform.Android) isMobile = true;
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
        Time.timeScale = 1f;
        mainMenuPanels.SetActive(false);
        if (isMobile) touchJoystick.SetActive(true);
    }


}