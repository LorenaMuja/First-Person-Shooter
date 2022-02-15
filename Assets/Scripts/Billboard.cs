using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera player;

    void Start()
    {

    }

   
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
