using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;

    Vector3 initialPosition;

    internal bool scrollingEnabled = true;
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(scrollingEnabled == true)
            NormalScroll();

        TestingScroll();
    }

    internal void Reset()
    {
        transform.position = initialPosition;
        scrollingEnabled = false;
    }

    void NormalScroll()
    {
        Vector3 pos = transform.position;
        pos.z -= speed * Time.deltaTime;
        transform.position = pos;
    }

    void TestingScroll()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.z -= 3 * Time.deltaTime;
            transform.position = pos;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.z += 3 * Time.deltaTime;
            transform.position = pos;
        }
    }

    
}
