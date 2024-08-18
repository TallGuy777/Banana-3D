using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomText : MonoBehaviour
{
    public RectTransform uiElementRectTransform; // Tham chiếu đến RectTransform của UI element
    [SerializeField] private GameObject DarkImage;
    [SerializeField] private GameObject[] textRan; // Danh sách các GameObject để bật/tắt ngẫu nhiên

    [SerializeField] private AudioSource DarkSoundEffect;

    private bool wasMouseOver = false; // Biến để theo dõi trạng thái của chuột

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Đảm bảo DarkImage và tất cả các textRan đều bị tắt khi scene được tải
        DarkImage.SetActive(false);
        HideAllText();
    }

    private void FixedUpdate()
    {
        if (IsMouseOverUI())
        {
            if (!wasMouseOver)
            {
                wasMouseOver = true;
                ShowRandomText();
                DarkImage.SetActive(true);

                // Phát âm thanh khi chuột di vào
                if (!DarkSoundEffect.isPlaying)
                {
                    DarkSoundEffect.Play();
                }
            }
        }
        else
        {
            if (wasMouseOver)
            {
                wasMouseOver = false;
                DarkImage.SetActive(false);
                HideAllText();

                // Ngừng phát âm thanh khi chuột rời đi
                DarkSoundEffect.Stop();
            }
        }
    }

    bool IsMouseOverUI()
    {
        // Lấy vị trí chuột trên màn hình
        Vector2 mousePosition = Input.mousePosition;

        // Chuyển đổi vị trí chuột từ màn hình sang không gian thế giới UI
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiElementRectTransform, mousePosition, null, out Vector2 localPoint);

        // Kiểm tra nếu điểm chuột nằm trong kích thước của RectTransform
        return uiElementRectTransform.rect.Contains(localPoint);
    }

    void ShowRandomText()
    {
        // Tắt tất cả các GameObject
        HideAllText();

        // Bật ngẫu nhiên một GameObject từ danh sách
        if (textRan.Length > 0)
        {
            int randomIndex = Random.Range(0, textRan.Length);
            textRan[randomIndex].SetActive(true);
        }
    }

    void HideAllText()
    {
        // Tắt tất cả các GameObject
        foreach (GameObject obj in textRan)
        {
            obj.SetActive(false);
        }
    }
}
