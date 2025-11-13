using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    Transform tf;
    int time = 0;

    void Start()
    {
        tf = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if(time < 10)
        {
            tf.Translate(0f, 0.1f, 0f);
            time ++;
        }
    }
}
