using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnDarkChar : MonoBehaviour
{
    [SerializeField] private List<Transform> ArrayListOfSpawnChar = new List<Transform>();
    [SerializeField] private GameObject DarkChar;

    void Start()
    {
        SpawnRandom();
    }

    void SpawnRandom()
    {
        if (ArrayListOfSpawnChar.Count == 0)
        {
            Debug.LogWarning("ArrayListOfSpawnChar is empty. No spawn locations available.");
            return;
        }

        // Chọn một vị trí ngẫu nhiên từ danh sách
        int randomIndex = Random.Range(0, ArrayListOfSpawnChar.Count);
        Transform spawnLocation = ArrayListOfSpawnChar[randomIndex];

        if (DarkChar != null && spawnLocation != null)
        {
            // Sinh đối tượng DarkChar tại vị trí đã chọn
            GameObject spawnedDarkChar = Instantiate(DarkChar, spawnLocation.position, spawnLocation.rotation);

            // Tìm và thiết lập thành phần DarkCharacterBase của đối tượng vừa sinh ra
            DarkCharacterBase darkCharacterBase = spawnedDarkChar.GetComponent<DarkCharacterBase>();
            if (darkCharacterBase != null)
            {
                // Bạn có thể thiết lập hoặc khởi tạo các tham số tại đây nếu cần
                Debug.Log(spawnedDarkChar.name + " đã được sinh ra tại vị trí " + spawnLocation);
            }
            else
            {
                Debug.LogError("DarkCharacterBase component is missing on the spawned DarkChar.");
            }
        }
        else
        {
            Debug.LogWarning("DarkChar or spawnLocation is null.");
        }
    }
}
