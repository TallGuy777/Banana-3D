using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkCharacterBase : MonoBehaviour
{
    public FieldOfView fieldOfView;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform PlayerTrans;
    [SerializeField] private RandomMove randomMoveScript;  // Tham chiếu tới RandomMove script
    [SerializeField] private float normalSpeed = 3.5f; // Normal speed of the agent
    [SerializeField] private float chaseSpeed = 7.0f; // Speed of the agent while chasing
    private bool isMovingToRandomPoint = false;
    [SerializeField] private float randomMoveDelay = 2f; // Thay đổi thời gian này theo ý bạn

    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        PlayerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        // Tìm RandomMove script trên scene
        if (randomMoveScript == null)
        {
            randomMoveScript = FindAnyObjectByType<RandomMove>();
        }

        // Nếu không tìm thấy, báo lỗi
        if (randomMoveScript == null)
        {
            Debug.LogError("Không tìm thấy RandomMove script trong scene!");
        }
        
    }

    private void Update()
    {
        if (fieldOfView.canSeePlayer)
        {
            ChasingPlayer();
        }
        else
        {
            if (!isMovingToRandomPoint)
            {
                navMeshAgent.speed = normalSpeed;
                StartCoroutine(MoveToRandomPoint());
            }
        }
    }

    public void ChasingPlayer()
    {
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(PlayerTrans.position);
    }

    private IEnumerator MoveToRandomPoint()
    {
        isMovingToRandomPoint = true;

        if (randomMoveScript.randomMovePoint.Count == 0)
        {
            Debug.LogWarning("RandomMovePoint list is empty. No random points available.");
            yield break;
        }

        // Chọn một vị trí ngẫu nhiên từ danh sách
        int randomIndex = Random.Range(0, randomMoveScript.randomMovePoint.Count);
        Transform spawnLocation = randomMoveScript.randomMovePoint[randomIndex];

        navMeshAgent.SetDestination(spawnLocation.position);
        Debug.Log(gameObject.name + " đang di chuyển tới vị trí " + spawnLocation.name);

        // Chờ cho đến khi đối tượng hoàn tất di chuyển đến điểm ngẫu nhiên
        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        // Đợi một khoảng thời gian trước khi tiếp tục di chuyển đến điểm ngẫu nhiên khác
        yield return new WaitForSeconds(randomMoveDelay);

        isMovingToRandomPoint = false;
    }
}
