using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    [SerializeField] private GameObject DeathCam;
    private void Awake()
    {
        DeathCam=GameObject.FindGameObjectWithTag("DeathCamera");
    }
    private void LateUpdate()
    {
        DeathCam.transform.position = transform.position;
        DeathCam.transform.rotation = transform.rotation;
    }
}
