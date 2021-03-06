﻿using JetBrains.Annotations;
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
    [SerializeField]
    PlayerWithCollider man = null;
    [SerializeField]
    Transform ground = null;
    [SerializeField]
    public bool useDebugColors = false;
    [SerializeField]
    float timeBetweenEachBoxSpawned = 0.18f;
    [SerializeField, Range(0.0f, 4.0f)]
    float boxesPerCorrectAnswer = 1;

    int numBoxesStagedToBeSpawned = 0;
    int boxIdForDebugging = 0;
    float timeForNextBoxSpawn = 0;
    //-----------------------------------------------------------------
    List<GameObject> boxesSpawned;
    private LayerMask raycastBoxLayer = 0, levelLayer = 0, supportBlockLayer =0;

    bool updateSpawnPosition = false;
    Vector3 positionTracker;
    float boxHeight = 0;

    void Start()
    {
        raycastBoxLayer = LayerMask.GetMask("Barriers");
        levelLayer = LayerMask.GetMask("Ground");
        supportBlockLayer = LayerMask.GetMask("SupportBlock");

        GetBasePositionAndBoxHeight();
        boxes.gameObject.SetActive(false);
        boxesSpawned = new List<GameObject>();

        
        //int value = 1<<LayerMask.NameToLayer("Ground");
    }

    private void Update()
    { 
        if(updateSpawnPosition)
            UpdateSpawnPosition();
    }

    void UpdateSpawnPosition()
    {
        RaycastHit hit;
        Vector3 pos = positionTracker;
        float gap = 8.2f;
        pos.y += gap;
        int combinedMask = (supportBlockLayer | levelLayer);

        if (Physics.Raycast(pos, Vector3.down, out hit, gap*2, combinedMask) == true)
        {
            int layer = hit.transform.gameObject.layer;
            //int blockMask = LayerMask.GetMask("SupportBlock");
            //int groundMask = LayerMask.NameToLayer("Ground");
            if (layer == LayerMask.NameToLayer("Ground"))
            {
                positionTracker = spawnPoint.transform.position;
            }
            else if(layer == LayerMask.NameToLayer("SupportBlock"))
            {
                positionTracker = hit.point;
                positionTracker.y += 0.05f;
            }
        }
    }

    void GetBasePositionAndBoxHeight()
    {
        boxHeight = boxes.GetComponent<BoxCollider>().bounds.extents.y * 2;

        positionTracker = spawnPoint.transform.position;
        RaycastHit hit;
        Physics.Raycast(positionTracker, Vector3.down, out hit, 2, levelLayer);
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

        float actualNum = boxesPerCorrectAnswer * numBoxes;
        numBoxesStagedToBeSpawned += (int)actualNum;
        //StartCoroutine("AddBoxTimed", (int) actualNum);
    }

    private void LateUpdate()
    {
        if (numBoxesStagedToBeSpawned > 0)
        {
            updateSpawnPosition = true;
            if (Utils.HasExpired(timeForNextBoxSpawn))
            {
                numBoxesStagedToBeSpawned--;
                GameObject obj = Instantiate(boxes, positionTracker, spawnPoint.transform.rotation);
                obj.SetActive(true);
                Utils.SetAllChildrenActive(obj, true);

                obj.name = "Box " + boxIdForDebugging++;
                obj.transform.parent = placeToStackBoxes;
                obj.GetComponent<RaycastBox>().Set(this);
                obj.GetComponent<Rigidbody>().isKinematic = false;
                obj.GetComponent<Rigidbody>().useGravity = true;


                timeForNextBoxSpawn = Time.time + timeBetweenEachBoxSpawned;
                positionTracker.y += boxHeight;
                man.BoxSpawned(positionTracker);
                boxesSpawned.Add(obj);
            }
        }
        else
        {
            updateSpawnPosition = false;
        }
    }
  /*  IEnumerator AddBoxTimed(int numBoxes)
    {
        updateSpawnPosition = false;
        for (int i=0; i<numBoxes; i++)
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
        yield return new WaitForSeconds(0.05f);
        //man.EnableGravity(false);
        updateSpawnPosition = true;
    }*/

    public void Reset()
    {
        CleanupBoxes();
    }
    void CleanupBoxes()
    {
        foreach(var box in boxesSpawned)
        {
            Destroy(box);
        }
        boxesSpawned.Clear();
    }

    internal void FrontDownwardRayHitCollider()
    {
        //Debug.Log("front ray hit collider");
        //man.EnableGravity(false);
        //isGravityOn = false;
    }
    internal void BackDownwardRayHitCollider()
    {
        //Debug.Log("back ray stopped touching collider");
        //man.EnableGravity(true);
        //isGravityOn = true;

        var listOfKin = GetComponentsInChildren<RaycastBox>();
        foreach (var box in boxesSpawned)
        {
            box.GetComponent<Rigidbody>().useGravity = true;
        }
        var gm = GameObject.FindObjectOfType<GameManager>();
        gm?.OnBoxesPassBarrier();
    }

    internal void BoxHitBarrier(RaycastBox box)
    {
        box.transform.parent = ground;
        box.isImmobile = true;
        box.GetComponent<Rigidbody>().useGravity = true;
    }

    internal  void BoxHitGround(RaycastBox box)
    {
        //return;
       // box.GetComponent<Rigidbody>().useGravity = false;
       // box.GetComponent<Rigidbody>().velocity = Vector3.zero;
      /*  foreach (var b in boxesSpawned)
        {
            b.GetComponent<Rigidbody>().useGravity = false;
        }*/
    }
    internal void BoxHitOtherBlock(RaycastBox box)
    {
        return;
        box.GetComponent<Rigidbody>().useGravity = false;
        box.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
