using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManagement : MonoBehaviour
{
    [SerializeField]
    GameObject collectablePreFab;

    List<Transform> listOfCoinLocations;
    List<GameObject> spawnedCoins;
    GameObject spawnLocation;
    void Start()
    {
        listOfCoinLocations = new List<Transform>();
        var listOfPotentials = GetComponentsInChildren <SphereCollider>();
        int collectableLayer = LayerMask.NameToLayer("Collectable");
        spawnedCoins = new List<GameObject>();
        GameObject coinParent = Utils.GetChildWithName(this.gameObject, "Coins");
        spawnLocation = new GameObject();
        Instantiate(spawnLocation, coinParent.transform);
        spawnLocation.transform.parent = coinParent.transform;

        foreach (var i in listOfPotentials)
        {
            if(i.gameObject.layer == collectableLayer)// nicely. only active coins show
            {
                Debug.Log(i.gameObject.name);
                listOfCoinLocations.Add(i.transform);
                i.gameObject.SetActive(false);
            }
        }
    }

    public void StartNewLevel()
    {
        foreach (var coin in spawnedCoins)
        {
            Destroy(coin);
        }
        spawnedCoins.Clear();

        
        int collectableLayer = LayerMask.NameToLayer("Collectable");
        foreach (var loc in listOfCoinLocations)
        {
            GameObject coin = Instantiate(collectablePreFab, loc.position, loc.rotation, spawnLocation.transform);
            coin.SetActive(true);
            coin.gameObject.layer = collectableLayer;
            //coin.transform.parent = coinParent.transform;
            spawnedCoins.Add(coin);
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}