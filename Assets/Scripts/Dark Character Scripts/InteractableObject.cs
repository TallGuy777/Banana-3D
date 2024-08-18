using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject DeathCamera;

    private void Awake()
    {
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (DeathCamera == null) DeathCamera = GameObject.FindGameObjectWithTag("DeathCamera");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng va chạm là người chơi
        if (other.CompareTag("Player"))
        {
            // Đặt trạng thái người chơi thành false
            Player.SetActive(false);

            // Tính toán hướng từ DeathCamera đến Player
            Vector3 direction = (transform.position - DeathCamera.transform.position).normalized;
            
            // Tạo một góc quay từ DeathCamera về hướng Player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            // Xoay DeathCamera về hướng Player
            DeathCamera.transform.rotation = lookRotation;
        }
    }
}
