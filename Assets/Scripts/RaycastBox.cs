using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//#define 
public class RaycastBox : MonoBehaviour
{
    float boxHeight = 0;
    private int supportBlockLayer, barrierLayer;
    public bool barrierIsHit { get; set; }

    Transform[] raycastPoints;
    [SerializeField]
    Transform raycastDownRear = null, raycastDownFront = null;
    void Start()
    {
        GetBasePositionAndBoxHeight();
        /*var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
        transforms.Remove(transform);
        raycastPoints = transforms.ToArray();*/
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
    bool DetectBelow(bool isFront)
    {
        RaycastHit hit;
        int layerMask = supportBlockLayer | barrierLayer;
        Transform transform = raycastDownFront;
        if (isFront == false)
            transform = raycastDownRear;


        float maxDist = 1;
        bool didHit = Physics.Raycast(transform.position, Vector3.down, out hit, maxDist, layerMask);

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
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if (didHit)
            return true;

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
        if (DetectBelow(true) == true)
            SetColor(Color.white);
        if (DetectBelow(false) == true)
            SetColor(Color.yellow);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == barrierLayer)
        {
            transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
