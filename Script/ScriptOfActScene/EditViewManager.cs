using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditViewManager : MonoBehaviour
{
    Transform tf;

    const float speed0 = 0.2f;
    float speed = speed0;

    void Start()
    {
        tf = GetComponent<Transform>();
        tf.position = new Vector3(13f, 15f, -10f);
    }

    public void MoveEditView()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = speed0 * 1.5f;
        }
        else
        {
            speed = speed0;
        }

        // 左に移動
        if (Input.GetKey (KeyCode.A) && tf.position.x > 13f) {
            tf.Translate (-speed,0f,0f);
        }
        // 右に移動
        if (Input.GetKey (KeyCode.D) && tf.position.x < 67f) {
            tf.Translate (speed,0f,0f);
        }
        // 上に移動
        if (Input.GetKey (KeyCode.W) && tf.position.y < 22.5f) {
            tf.Translate (0f,speed,0f);
        }
        // 下に移動
        if (Input.GetKey (KeyCode.S) && tf.position.y > 7.5f) {
            tf.Translate (0f,-speed,0f);
        }
    }
}
