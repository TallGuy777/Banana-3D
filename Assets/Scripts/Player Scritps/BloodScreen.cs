using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this to access RawImage

public class BloodScreen : MonoBehaviour
{
    public RawImage bloodImage; // Reference to the RawImage component
    public float fadeDuration = 2f; // Duration for the fade effect
    [SerializeField] private GameObject Player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider box;
    [SerializeField] private AudioListener DeathCamera;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        DeathCamera = GetComponent<AudioListener>();
        SetImageAlpha(0f);
    }

    private void FixedUpdate()
    {
        if (Player.activeInHierarchy)
        {
            box.enabled = false;
            DeathCamera.enabled = false;
            rb.isKinematic = true;
            SetImageAlpha(0f);
        }
        else
        {
            box.enabled = true;
            DeathCamera.enabled = true;
            rb.isKinematic = false;
            StartCoroutine(FadeInBloodScreen());
        }
    }
    private IEnumerator FadeInBloodScreen()
    {
        float elapsedTime = 0f;
        Color currentColor = bloodImage.color;

        // Fade in from 0% to 100%
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }

        // Ensure final alpha is set to 100%
        SetImageAlpha(1f);
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = bloodImage.color;
        color.a = alpha;
        bloodImage.color = color;
    }
}
