using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText; // Tham chiếu đến TextMeshProUGUI trên UI
    [SerializeField] private float typingSpeed = 0.05f; // Thời gian giữa các ký tự
    [SerializeField] private GameObject LoadMapButton; // Tham chiếu đến LoadMapButton

    private bool isTyping = false; // Kiểm tra xem có đang gõ chữ không
    private string fullText; // Biến lưu toàn bộ văn bản
    private Coroutine typingCoroutine;

    private void Start()
    {
        LoadMapButton.SetActive(false); // Đảm bảo LoadMapButton ẩn khi bắt đầu
        string message = "Bạn đã quá nhiều chuối nên bạn đã bị triệu hồi đến địa ngục của chuối, nơi có một con quỷ chuối sẽ trừng trị bạn bằng 'cái ấy' của hắn ta vì bạn đã ăn quá nhiều chuối.";
        typingCoroutine = StartCoroutine(TypeText(message));
    }

    private void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            // Nếu người dùng nhấn phím khi đang gõ chữ, dừng Coroutine và hiển thị toàn bộ văn bản
            StopCoroutine(typingCoroutine);
            uiText.text = fullText; // Hiển thị toàn bộ văn bản
            isTyping = false; // Đánh dấu là không còn gõ chữ nữa
            LoadMapButton.SetActive(true); // Hiển thị LoadMapButton
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true; // Đánh dấu là đang gõ chữ
        uiText.text = ""; // Xóa văn bản ban đầu
        fullText = text; // Lưu văn bản đầy đủ vào biến fullText

        foreach (char letter in text.ToCharArray())
        {
            uiText.text += letter; // Thêm từng ký tự một vào TextMeshProUGUI
            yield return new WaitForSeconds(typingSpeed); // Đợi một khoảng thời gian giữa các ký tự

            if (!isTyping)
            {
                yield break; // Nếu đã nhấn phím để bỏ qua, thoát khỏi vòng lặp
            }
        }

        isTyping = false; // Hoàn tất việc gõ chữ
        LoadMapButton.SetActive(true); // Hiển thị LoadMapButton sau khi văn bản đã được gõ xong
    }

    public void LoadBananaMap()
    {
        // Tải scene mới có tên "BananaMap"
        SceneManager.LoadScene("BananaMap");
    }
}
