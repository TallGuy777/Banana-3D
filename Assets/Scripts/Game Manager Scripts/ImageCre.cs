using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCre : MonoBehaviour
{
    void Update()
    {
        Animator animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
}
