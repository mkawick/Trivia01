using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    GameObject runway = null;

    Vector3 initialPosition;
    Vector3 runwayInitialPosition;

    internal bool scrollingEnabled = true;
    public bool useRunway = false;
    void Start()
    {
        initialPosition = transform.position;
        if(runway != null)
        {
            GameObject runwayStart = Utils.GetChildWithName(runway, "RunwayStart");
            runwayInitialPosition = runwayStart.transform.position;
            runwayInitialPosition.y = initialPosition.y;
            runwayStart.transform.position = runwayInitialPosition;
            if (useRunway)
            {
                transform.position -= runwayInitialPosition;
                initialPosition = transform.position;
                runway.transform.position -= runwayInitialPosition;
            }
        }
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
        float offset = speed * Time.deltaTime;
        Vector3 pos = transform.position;
        pos.z -= offset;
        transform.position = pos;
        if (runway != null)
        {
            pos = runway.transform.position;
            pos.z -= offset;
            runway.transform.position = pos;
        }
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
