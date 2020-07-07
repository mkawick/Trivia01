using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithCollider : MonoBehaviour
{
    internal bool initialState = true;

    [SerializeField]
    bool playerTester = false;

    public int score = 0;

    internal void Reset()
    {
        initialState = true;
    }

    internal void NormalGameplay()
    {
        initialState = false;
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
            var gm = GameObject.Find("GameManager");
            if (gm)
            {
                gm.GetComponent<GameManager>().OnPlayerTouchesDown();
            }
        }
        
        mask = LayerMask.NameToLayer("Barriers");
        if (collision.gameObject.layer == mask)
        {
            var gm = GameObject.Find("GameManager");
            if (gm)
            {
                gm.GetComponent<GameManager>().OnPlayerHitsBarrier();
            }
        }
    }

    public void BoxSpawned(Vector3 locationOfBox)
    {
        Vector3 verticalOffset = new Vector3(0, 1.2f, 0);
        this.transform.position = locationOfBox + verticalOffset;
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
