using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerTester : MonoBehaviour
{
    public BoxStacker stacker;
    public Transform man;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) == true)
        {
            stacker.AddBox(3);
            Vector3 top = stacker.GetHighestPoint();
            top.y++;
            Vector3 pos = man.transform.position;
            pos = top;
            man.transform.position = pos;
        }
    }
}
