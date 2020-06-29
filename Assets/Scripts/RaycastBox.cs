using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//#define 
public class RaycastBox : MonoBehaviour
{
    /*[SerializeField]
    GameObject boxes = null;*/
    /* [SerializeField]
     Color[] colors;*/
    float boxHeight = 0;
    private int supportBlockLayer, barrierLayer;
    public bool barrierIsHit { get; set; }

    Transform[] raycastPoints;
    void Start()
    {
        GetBasePositionAndBoxHeight();
        var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
        transforms.Remove(transform);
        raycastPoints = transforms.ToArray();
        supportBlockLayer = LayerMask.NameToLayer("SupportBlock");
        barrierLayer = LayerMask.NameToLayer("Barriers");
        barrierIsHit = false;
    }

    void GetBasePositionAndBoxHeight()
    {
        boxHeight = GetComponent<MeshRenderer>().bounds.extents.y * 2;

        //positionTracker = spawnPoint.transform.position;
      /*  RaycastHit hit;
        Physics.Raycast(positionTracker, Vector3.down, out hit, 2);
        // add 1/2 height to it.
        positionTracker = hit.point;
        positionTracker.y += boxHeight / 2 + 0.01f;*/
    }

    bool DetectBarrierAhead()
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, Vector3.forward, out hit, 2);

        Debug.DrawRay(transform.position, Vector3.forward, Color.yellow);
        if (didHit)
        {
            barrierIsHit = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
        return didHit;
    }
    bool DetectBelow()
    {
        RaycastHit hit;
        int layerMask = supportBlockLayer | barrierLayer;
        foreach (var ray in raycastPoints)
        {
            bool didHit = Physics.Raycast(ray.transform.position, Vector3.down, out hit, layerMask);

            if (didHit)
            {
                if (hit.transform.gameObject.layer == supportBlockLayer)
                {
                    Debug.Log("support");

                    // Make a path
                }
                else
                {
                    Debug.Log("barrierLayer");
                    // Do whatever you want
                }
            }
            Debug.DrawRay(ray.transform.position, Vector3.down, Color.red);
            if (didHit)
                return true;
        }
        return false;
    }

    void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    private void Update()
    {
        if (DetectBarrierAhead() == true)
            SetColor(Color.blue);
        if (DetectBelow() == true)
            SetColor(Color.white);
    }

    // Update is called once per frame
    /*  public Vector3 GetHighestPoint()
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
      }*/
}
