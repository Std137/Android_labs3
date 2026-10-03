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

    private GameObject Wall(int x, int y, bool rotate)
    {
        var localScaleX = (rotate) ? 10 : 1;
        var localScaleZ = (rotate) ? 1 : 10;
        var localPositionX = x * 10.0f;
        var localPositionY = y * 10.0f;
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.localScale = new Vector3(localScaleX, 20, localScaleZ);
        wall.transform.position = new Vector3(localPositionX, 10.0f, localPositionY);
        wall.AddComponent<BoxCollider>();
        Material wallMaterial = new Material();
        return wall;
    }

    public void Start()
    {
        mainMenuPanels.SetActive(true);

        Time.timeScale = 0f;
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




}