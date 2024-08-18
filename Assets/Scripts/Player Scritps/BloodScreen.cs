using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class BloodScreen : MonoBehaviour
{
    public RawImage bloodImage; // Reference to the RawImage component
    public float fadeDuration = 2f; // Duration for the fade effect
    [SerializeField] private GameObject Player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider box;
    [SerializeField] private AudioListener DeathCamera;
    [SerializeField] private AudioListener Maincamera;
    [SerializeField] private AudioSource Screaming,ScareSound;

    [SerializeField] private GameObject LostGameUI;
    [SerializeField] private FirstPersonController FirstPerson;
    private bool hasPlayedScreaming = false; // Flag to check if Screaming has been played

    void Awake()
    {
        // Tìm và gán các thành phần nếu chưa được gán từ Inspector
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (box == null) box = GetComponent<BoxCollider>();

        // Tìm các AudioListener trong scene
        if (DeathCamera == null) DeathCamera = FindObjectOfType<AudioListener>();
        if (Maincamera == null) Maincamera = FindObjectOfType<AudioListener>();

        if (DeathCamera == null || Maincamera == null)
        {
            Debug.LogError("Không tìm thấy AudioListener trong scene. Hãy đảm bảo có ít nhất một AudioListener.");
        }

        FirstPerson = FindAnyObjectByType<FirstPersonController>();

        SetImageAlpha(0f);
        DeathCamera.enabled = false;
        Maincamera.enabled = true;
    }

    private void Update()
    {
        if (Player != null)
        {
            if (Player.activeInHierarchy)
            {
                // Khi Player còn hoạt động
                SetImageAlpha(0f);
                box.enabled = false;
                if (DeathCamera != null) DeathCamera.enabled = false;
                if (Maincamera != null) Maincamera.enabled = true;
                rb.isKinematic = true;
                LostGameUI.SetActive(false);
                // Reset flag khi Player trở lại hoạt động
                hasPlayedScreaming = false;
                UnityEngine.Cursor.visible = false;
            }
            else
            {
                // Khi Player không còn hoạt động
                if (!hasPlayedScreaming)
                {
                    Screaming.Play();
                    ScareSound.Play();
                    hasPlayedScreaming = true; // Set flag to true to prevent replay
                }

                box.enabled = true;
                if (DeathCamera != null) DeathCamera.enabled = true;
                if (Maincamera != null) Maincamera.enabled = false;
                rb.isKinematic = false;
                LostGameUI.SetActive(true);
                StartCoroutine(FadeInBloodScreen());
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
        }
    }

    private IEnumerator FadeInBloodScreen()
    {
        float elapsedTime = 0f;
        Color currentColor = bloodImage.color;

        // Fade in từ 0% đến 100%
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }

        // Đảm bảo alpha cuối cùng là 100%
        SetImageAlpha(1f);
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = bloodImage.color;
        color.a = alpha;
        bloodImage.color = color;
    }
}
