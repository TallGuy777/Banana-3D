using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu, Setting;
    [SerializeField] private Slider Music;
    [SerializeField] private AudioSource musicSource, mrIncredibleDark; // Nguồn phát nhạc

    //FPS
    int MaxRate = 9999;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;

    private void Awake()
    {
        // Hiển thị con trỏ chuột trong Main Menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Tải giá trị âm lượng từ PlayerPrefs nếu có, nếu không, sử dụng giá trị mặc định 1.0f
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        if (Music != null && musicSource != null && mrIncredibleDark != null)
        {
            Music.value = savedVolume;
            musicSource.volume = savedVolume;
            mrIncredibleDark.volume = savedVolume; // Áp dụng giá trị đã lưu vào âm lượng của musicSource
        }
        else
        {
            Debug.LogWarning("Music slider or source is null.");
        }
        Menu.SetActive(true);
        Setting.SetActive(false);
        //FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }

    private void FixedUpdate()
    {
        SettingMusic();
    }

    //FPS
    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
            {
                Thread.Sleep((int)(sleepTime * 1000));
                while (t < currentFrameTime)
                {
                    t = Time.realtimeSinceStartup;
                }
            }
        }
    }

    //Menu Game
    public void StartGame()
    {
        SceneManager.LoadScene("BananaMap");
    }

    public void SettingMenu()
    {
        if (!Setting.activeInHierarchy && Menu.activeInHierarchy)
        {
            Menu.SetActive(false);
            Setting.SetActive(true);
        }
    }

    public void ExitSettingGame()
    {
        if (Setting.activeInHierarchy && !Menu.activeInHierarchy)
        {
            Menu.SetActive(true);
            Setting.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SettingMusic()
    {
        if (Music != null && musicSource != null)
        {
            float volume = Music.value; // Lấy giá trị từ slider
            musicSource.volume = volume;
            mrIncredibleDark.volume = volume;
            // Lưu giá trị âm lượng vào PlayerPrefs
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save(); // Lưu ngay lập tức

            if (volume == 0)
            {
                musicSource.mute = true; // Tắt nhạc nếu âm lượng bằng 0
            }
            else
            {
                musicSource.mute = false; // Bật nhạc nếu âm lượng lớn hơn 0
            }
        }
    }
}
