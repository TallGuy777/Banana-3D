using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI CollectionText;
    public GameMenu gameMenu;
    [SerializeField] private GameObject WinGameUI;
    [SerializeField] private FirstPersonController FirstPerson;
    private int amountItem;

    void Awake()
    {
        FirstPerson = FindAnyObjectByType<FirstPersonController>();
        gameMenu = FindAnyObjectByType<GameMenu>();
    }

    private void Update()
    {
        if (amountItem == 6)
        {
            WinGameUI.SetActive(true);
            Time.timeScale = 0;
            FirstPerson.enabled = false;
            
        }
        else if (amountItem <= 6)
        {
            WinGameUI.SetActive(false);
            Time.timeScale = 1;
            FirstPerson.enabled = true;
            Cursor.visible = true;
        }
    }

    public void UpdateCollection(int amount)
    {
        // Cộng dồn amount vào amountItem
        amountItem += amount;

        // Cập nhật số lượng vật phẩm thu thập và hiển thị lên UI
        CollectionText.SetText("Banana " + amountItem + "/6");

        Debug.Log("Collected: " + amountItem);
    }
}
