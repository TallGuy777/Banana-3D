using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private FirstPersonController FirstPerson;
    [SerializeField] private GameObject MenuInGame, SettingInGame, FpsUI;
    [SerializeField] private Toggle CheckShowFPS;
    [SerializeField] private TextMeshProUGUI ExitTextTroll;

    private void Awake()
    {
        FirstPerson = FindAnyObjectByType<FirstPersonController>();
        MenuInGame.SetActive(false);
        SettingInGame.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ẩn FpsUI khi mới vào game
        FpsUI.SetActive(false);
        ResumeGame();
    }

    void Start()
    {

        bool savedShowFPS = PlayerPrefs.GetInt("ShowFPS", 0) == 1;
        if (CheckShowFPS != null)
        {
            CheckShowFPS.isOn = savedShowFPS;
            UpdateObjectVisibility(savedShowFPS);
            CheckShowFPS.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MenuInGame.activeInHierarchy && !SettingInGame.activeInHierarchy)
            {
                OpenMenuGame();
            }
            else if (MenuInGame.activeInHierarchy && !SettingInGame.activeInHierarchy)
            {
                CloseMenuGame();
            }
        }
    }

    // Chuột trong game
    void CursorShow()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CursorHide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Hiện thị FPS
    private void OnToggleValueChanged(bool isOn)
    {
        UpdateObjectVisibility(isOn);
        // Lưu trạng thái của FPS vào PlayerPrefs
        PlayerPrefs.SetInt("ShowFPS", isOn ? 1 : 0);
        PlayerPrefs.Save(); // Lưu ngay lập tức
    }

    // Cập nhật sự hiện thị của targetObject
    private void UpdateObjectVisibility(bool isVisible)
    {
        if (FpsUI != null)
        {
            FpsUI.SetActive(isVisible);
        }
    }

    // Menu và setting
    public void OpenMenuGame()
    {
        MenuInGame.SetActive(true);
        SettingInGame.SetActive(false);
        Time.timeScale = 0;
        FirstPerson.enabled = false;
        CursorShow();
    }

    public void CloseMenuGame()
    {
        MenuInGame.SetActive(false);
        SettingInGame.SetActive(false);
        Time.timeScale = 1;
        FirstPerson.enabled = true;
        CursorHide();
    }

    public void ResumeGame()
    {
        CloseMenuGame();
    }

    public void BackToMenu()
    {
        FirstPerson.enabled = false;
        SceneManager.LoadScene("MenuMain");
        CursorShow();
    }

    public void OpenSettingGame()
    {
        if (MenuInGame.activeInHierarchy && !SettingInGame.activeInHierarchy)
        {
            MenuInGame.SetActive(false);
            SettingInGame.SetActive(true);
            FirstPerson.enabled = false;
            CursorShow();
        }
    }

    public void ExitSetting()
    {
        if (!MenuInGame.activeInHierarchy && SettingInGame.activeInHierarchy)
        {
            MenuInGame.SetActive(true);
            SettingInGame.SetActive(false);
            CursorShow();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("BananaMap");
        ResumeGame();
        Cursor.visible = false;
    }
    public void ExitTroll()
    {
        ExitTextTroll.SetText("You can't exit bruh...");
    }
}
