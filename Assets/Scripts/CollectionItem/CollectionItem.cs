using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionItem : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Khi người chơi thu thập vật phẩm, tăng điểm và cập nhật UI
            gameManager.UpdateCollection(1);

            // Hủy vật phẩm sau khi đã thu thập
            Destroy(gameObject);
        }
    }
}
