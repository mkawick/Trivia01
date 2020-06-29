using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#define 
public class BoxStacker : MonoBehaviour
{
    [SerializeField]
    GameObject boxes = null;
    [SerializeField]
    Color[] colors;
    [SerializeField]
    GameObject spawnPoint = null;
    [SerializeField]
    Transform placeToStackBoxes = null;
    Vector3 positionTracker;
    float boxHeight = 0;

    [SerializeField]
    float timeBetweenEachBoxSpawned = 0.15f;

    List<GameObject> boxesSpawned;
    void Start()
    {
        GetBasePositionAndBoxHeight();
        boxesSpawned = new List<GameObject>();
    }

    void GetBasePositionAndBoxHeight()
    {
        boxHeight = boxes.GetComponent<MeshRenderer>().bounds.extents.y * 2;

        positionTracker = spawnPoint.transform.position;
        RaycastHit hit;
        Physics.Raycast(positionTracker, Vector3.down, out hit, 2);
        // add 1/2 height to it.
        positionTracker = hit.point;
        positionTracker.y += boxHeight / 2 + 0.01f;
    }

    // Update is called once per frame
    public Vector3 GetHighestPoint()
    {
        return positionTracker;
    }

    public void  AddBox(int numBoxes)
    {
        if (numBoxes == 0)
            return;

        StartCoroutine("AddBoxTimed", numBoxes);
    }
    IEnumerator AddBoxTimed(int numBoxes)
    {
        for(int i=0; i<numBoxes; i++)
        {
            //Vector3 position = positionTracker;
            GameObject obj = Instantiate(boxes, positionTracker, spawnPoint.transform.rotation);
            positionTracker.y += boxHeight;
            boxesSpawned.Add(obj);
            //obj.transform.parent = this.transform;
            obj.transform.parent = placeToStackBoxes;
            yield return new WaitForSeconds(timeBetweenEachBoxSpawned);
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = false;
        }
    }
   
    void CleanupBoxes()
    {
        foreach(var box in boxesSpawned)
        {
            Destroy(box);
        }
        boxesSpawned.Clear();
    }
}
