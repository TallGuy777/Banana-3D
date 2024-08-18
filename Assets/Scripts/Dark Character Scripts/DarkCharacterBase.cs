using System.Collections;
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
    [SerializeField] private float randomMoveDelay = 2f; // Thay đổi thời gian này theo ý bạn
    [SerializeField] private float additionalDistance = 5.0f; // Khoảng cách thêm vào khi đuổi theo người chơi
    [SerializeField] private AudioSource chaseAudioSource; // Âm thanh phát ra khi đuổi theo

    private bool isMovingToRandomPoint = false;
    private bool isChasingPlayer = false;
    private Vector3 lastKnownPlayerPosition;

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

        // Kiểm tra và thiết lập AudioSource
        if (chaseAudioSource == null)
        {
            chaseAudioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (fieldOfView.canSeePlayer)
        {
            isChasingPlayer = true;
            lastKnownPlayerPosition = PlayerTrans.position;
            ChasingPlayer();
        }
        else if (isChasingPlayer)
        {
            StartCoroutine(GoToLastKnownPosition());
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
        if (!chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Play(); // Bắt đầu phát âm thanh khi đuổi theo
        }

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(PlayerTrans.position);
    }

    private IEnumerator GoToLastKnownPosition()
    {
        // Tính toán vị trí đích mới với khoảng cách thêm vào
        Vector3 directionToPlayer = (PlayerTrans.position - lastKnownPlayerPosition).normalized;
        Vector3 destination = lastKnownPlayerPosition + directionToPlayer * additionalDistance;

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(destination);

        // Đợi cho đến khi đối tượng hoàn tất di chuyển đến vị trí mất dấu
        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        // Dừng phát âm thanh khi không còn đuổi theo
        if (chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
        }

        // Sau khi đến vị trí mất dấu, tiếp tục các chức năng khác như di chuyển đến điểm ngẫu nhiên
        isChasingPlayer = false;
        yield return new WaitForSeconds(randomMoveDelay);

        // Sau khi đã hoàn tất việc di chuyển đến vị trí mất dấu, bắt đầu di chuyển đến điểm ngẫu nhiên
        if (!isMovingToRandomPoint)
        {
            navMeshAgent.speed = normalSpeed;
            StartCoroutine(MoveToRandomPoint());
        }
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
