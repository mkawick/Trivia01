using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringRegionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var regions = GetComponentsInChildren<ScoringRegion>();
        foreach(var region in regions)
        {
            region.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
