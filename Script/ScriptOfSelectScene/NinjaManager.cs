using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaManager : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3Int(-10, -2, 0);
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0.02f, 0f, 0f);
    }
}
