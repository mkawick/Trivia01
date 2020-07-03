using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    Transform playerStart = null;
    [SerializeField]
    GameObject runway = null;

    Vector3 initialPosition;
    Vector3 runwayStartPosition;

    internal bool scrollingEnabled = true;
    public bool useRunway = false;
    void Start()
    {
        initialPosition = transform.position;
        if(runway != null)
        {
            Debug.Assert(playerStart != null, "must assign player start for the runway feature");
            GameObject runwayStart = Utils.GetChildWithName(runway, "RunwayStart");
            runwayStartPosition = runwayStart.transform.position;
            runwayStartPosition -= playerStart.position;
            runwayStartPosition.y = initialPosition.y;

            runwayStart.transform.position = runwayStartPosition;
            if (useRunway)
            {
                runwayStartPosition.y = 0;
                transform.position -= runwayStartPosition;
                initialPosition = transform.position;
                runway.transform.position -= runwayStartPosition;
                runwayStartPosition = runway.transform.position;
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
        if (runway != null)
        {
            runway.transform.position = runwayStartPosition;
        }
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
