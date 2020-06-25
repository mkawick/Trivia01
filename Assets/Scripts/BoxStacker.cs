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
        boxHeight = boxes[0].GetComponent<MeshRenderer>().bounds.extents.y*2;
    }

    // Update is called once per frame
    public Vector3 GetHighestPoint()
    {
        return positionTracker;
    }
    public void AddBox(int numBoxes)
    {
        for(int i=0; i<numBoxes; i++)
        {
            Vector3 position = positionTracker;
            GameObject obj = Instantiate(boxes[0], positionTracker, spawnPoint.transform.rotation);
            positionTracker.y += boxHeight;
        }
    }
}
