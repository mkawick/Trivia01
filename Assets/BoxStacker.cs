using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#define 
public class BoxStacker : MonoBehaviour
{
    [SerializeField]
    GameObject[] boxes;
    [SerializeField]
    Color[] colors;
    [SerializeField]
    GameObject spawnPoint;
    Vector3 positionTracker;
    float boxHeight = 0;
    void Start()
    {
        positionTracker = spawnPoint.transform.position;
        boxHeight = boxes[0].GetComponent<MeshRenderer>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)== true)
        {
            Vector3 position = positionTracker;            
            GameObject obj = Instantiate(boxes[0], positionTracker, spawnPoint.transform.rotation);
            positionTracker.y += boxHeight;
        }
    }
}
