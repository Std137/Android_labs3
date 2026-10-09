using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [Header("Панели")]
    public GameObject mainMenuPanels;
    public GameObject exitPanel;
    public GameObject aboutPanel;
    public GameObject originaWall;
    public GameObject touchJoystick;

    private bool isMobile = true;
    private LelelFieldGenerate lg = new LelelFieldGenerate();

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
        StartAction();
        Time.timeScale = 1f;
        mainMenuPanels.SetActive(false);
        if (isMobile) touchJoystick.SetActive(true);
    }
    
    public void StartAction() {
        Quaternion rotateWall = Quaternion.Euler(0f, 90f, 0f);
        Quaternion norotateWall = Quaternion.Euler(0f, 0f, 0f);
        GameObject cloneWall;
        var originalWall = GameObject.FindGameObjectWithTag("WallTemplate");
        var originalLetter = GameObject.FindGameObjectWithTag("LetterTemplate");
        var levelObj = GameObject.FindGameObjectWithTag("LevelObj").transform;

        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++)
            {
                if (lg.GetRight(x, y)) 
                {
                    cloneWall = Object.Instantiate(originalWall, new Vector3((-30f+10*y), 10f, (35f-10*x)), norotateWall);
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
}