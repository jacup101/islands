using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    [field: SerializeField] private Animation anim;
    void Start()
    {
        anim.Play();
    }

}
