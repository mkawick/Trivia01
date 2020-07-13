using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithCollider : MonoBehaviour
{
    internal bool initialState = true;

    [SerializeField]
    bool playerTester = false;

    public int score = 0;
    internal CameraSwingAndZoom cameraSwingAndZoom;
    float timerToExitInitialState = 0;

    internal void Reset()
    {
        initialState = true;
        timerToExitInitialState = Time.time + 2;
    }

    internal void NormalGameplay()
    {
        initialState = false;
    }

    private void Update()
    {
        if(initialState == true)
        {
            if (Utils.HasExpired(timerToExitInitialState) == true)
                initialState = false;
        }
        if (DetectBarrierBelow())
        {
            EnableGravity(false);
        }
        else
        {
            EnableGravity(true);
        }

    }

    bool DetectBarrierBelow()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Barriers");

        float maxDist = 20;
        bool didHit = Physics.Raycast(transform.position, Vector3.down, out hit, maxDist, layerMask);

        if (didHit)
        {
            int layer = 1 << hit.transform.gameObject.layer;
        }
        Debug.DrawLine(transform.position, transform.position + Vector3.down * maxDist, Color.red);
        return didHit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int mask = LayerMask.NameToLayer("SupportBlock");
        if (collision.gameObject.layer == mask)
        {
            initialState = false;
            return;
        }
        mask = LayerMask.NameToLayer("Collectable");
        if (collision.gameObject.layer == mask)
        {
            score++;
            Debug.Log("score :" + score);
            Destroy(collision.gameObject);
            var gm = GameObject.Find("GameManager");
            if (gm)
            {
                gm.GetComponent<GameManager>().OnScoreChange(score);
            }
        }

        if (initialState && playerTester == false)
            return;

        mask = LayerMask.NameToLayer("Ground");
        if (collision.gameObject.layer == mask)
        {
            HandleTouchDown();
        }
        
        mask = LayerMask.NameToLayer("Barriers");
        if (collision.gameObject.layer == mask)
        {
            if (cameraSwingAndZoom != null)
            {
                cameraSwingAndZoom.BeginSadnessState(4);
            }
            var gm = GameObject.Find("GameManager");
            if (gm)
            {
                gm.GetComponent<GameManager>().OnPlayerHitsBarrier();
            }
        }
    }

    void HandleTouchDown()
    {
        /* var gm = GameObject.Find("GameManager");
         if (gm)
         {
             RaycastHit hit;
             int layerMask = LayerMask.GetMask("ScoringRegion");

             float maxDist = 1;
             bool didHit = Physics.Raycast(transform.position, Vector3.down, out hit, maxDist, layerMask);

             string text = "";
             if (didHit)
             {
                 text = hit.collider.gameObject.GetComponent<ScoringRegion>().awardText;
             }
             gm.GetComponent<GameManager>().OnPlayerTouchesDown(text);
         }*/
        /* */
        GetComponent<PlayerAnimController>().StartRunningState(true);
    }

    public void BoxSpawned(Vector3 locationOfBox)
    {
        Vector3 verticalOffset = new Vector3(0, 1.2f, 0);
        this.transform.position = locationOfBox + verticalOffset;
        GetComponent<PlayerAnimController>().StartRunningState(false);
    }
   /* bool DetectBarrierAhead()
    {
        int mask = LayerMask.NameToLayer("Barriers");
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, Vector3.forward, out hit, 0.2f, mask);

        if (didHit)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.OnPlayerHitsBarrier();
        }
        return didHit;
    }*/

    internal void EnableGravity(bool enable)
    {
        GetComponent<Rigidbody>().useGravity = enable;
        if(enable == false)
            GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
