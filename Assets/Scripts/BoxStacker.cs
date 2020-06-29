﻿using System.Collections;
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
    [SerializeField]
    PlayerWithCollider man = null;

    Vector3 positionTracker;
    float boxHeight = 0;

    [SerializeField]
    float timeBetweenEachBoxSpawned = 0.15f;

    List<GameObject> boxesSpawned;
    
    void Start()
    {
        GetBasePositionAndBoxHeight();
        boxes.gameObject.SetActive(false);
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
            obj.SetActive(true);
            positionTracker.y += boxHeight;
            boxesSpawned.Add(obj);
            //obj.transform.parent = this.transform;
            obj.name = "Box " + i;
            obj.transform.parent = placeToStackBoxes;
            obj.GetComponent<RaycastBox>().boxStacker = this;
            yield return new WaitForSeconds(timeBetweenEachBoxSpawned);
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = false;
        }
        /*yield return new WaitForSeconds(0.05f);
        man.EnableGravity(false);*/
    }
   
    void CleanupBoxes()
    {
        foreach(var box in boxesSpawned)
        {
            Destroy(box);
        }
        boxesSpawned.Clear();
    }

    internal void FrontRayHitCollider()
    {
        Debug.Log("front ray hit collider");
        man.EnableGravity(false);
    }
    internal void BackRayHitCollider()
    {
        Debug.Log("front ray hit collider");
        man.EnableGravity(true);
    }
}
