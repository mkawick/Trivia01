using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithCollider : MonoBehaviour
{
    internal bool initialState = true;

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
        if (initialState)
            return;
        mask = LayerMask.NameToLayer("Ground");
        if (collision.gameObject.layer == mask)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.OnPlayerTouchesDown();
        }
        
        mask = LayerMask.NameToLayer("Barriers");
        if (collision.gameObject.layer == mask)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.OnPlayerHitsBarrier();
        }
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
