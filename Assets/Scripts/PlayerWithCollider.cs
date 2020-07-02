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
        // game over on BarrierCollider
        // Celebration at the end zones
        if (initialState)
            return;
        int mask = LayerMask.NameToLayer("Ground");
        if (collision.gameObject.layer == mask)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.OnPlayerTouchesDown();
        }
        mask = LayerMask.NameToLayer("Barriers");
        if (collision.gameObject.layer == mask)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.OnPlayerTouchesDown();// todo
        }
    }

    internal void EnableGravity(bool enable)
    {
        GetComponent<Rigidbody>().useGravity = enable;
        if(enable == false)
            GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
