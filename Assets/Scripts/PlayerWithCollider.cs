using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // game over on BarrierCollider
        // Celebration at the end zones
    }

    internal void EnableGravity(bool enable)
    {
        GetComponent<Rigidbody>().useGravity = enable;
    }
}
