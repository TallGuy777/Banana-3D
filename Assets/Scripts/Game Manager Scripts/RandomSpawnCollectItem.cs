using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnCollectItem : MonoBehaviour
{
    public GameObject bananaPrefab;  // Prefab của chuối
    public List<Transform> spawnPoints;  // Danh sách các vị trí spawn
    public int numberOfBananas = 6;  // Số lượng chuối cần spawn

    void Start()
    {
        // Gọi hàm để spawn chuối
        SpawnBananas();
    }

    void SpawnBananas()
    {
        // Kiểm tra nếu số lượng chuối cần spawn vượt quá số lượng vị trí có sẵn
        if (numberOfBananas > spawnPoints.Count)
        {
            Debug.LogError("Không có đủ vị trí spawn cho số lượng chuối cần spawn.");
            return;
        }

        // Tạo một danh sách tạm để lưu trữ các vị trí spawn chưa được sử dụng
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        // Vòng lặp để spawn từng chuối
        for (int i = 0; i < numberOfBananas; i++)
        {
            // Chọn một vị trí ngẫu nhiên trong danh sách availableSpawnPoints
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            // Spawn chuối tại vị trí đã chọn
            Instantiate(bananaPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Spawned banana at position: {spawnPoint.position}");

            // Xóa vị trí đã sử dụng khỏi danh sách để đảm bảo không trùng lặp
            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}
