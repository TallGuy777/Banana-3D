using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();

            // Thêm phần này để đối tượng luôn hướng về target nếu nhìn thấy target
            if (canSeePlayer)
            {
                RotateTowardsTarget();
            }
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void RotateTowardsTarget()
    {
        // Lấy hướng từ đối tượng đến target, nhưng chỉ xét đến các giá trị trên mặt phẳng XZ (loại bỏ thành phần Y)
        Vector3 direction = (playerRef.transform.position - transform.position).normalized;
        direction.y = 0f;  // Đảm bảo rằng không có thành phần Y trong hướng này

        // Kiểm tra xem hướng có hợp lệ không (tránh trường hợp góc quay bằng 0 hoặc vô hạn)
        if (direction != Vector3.zero)
        {
            // Tính toán góc quay cần thiết để hướng về target chỉ trên trục Y
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Xoay đối tượng từ từ về phía target
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.time * 5f);
        }
    }

}
