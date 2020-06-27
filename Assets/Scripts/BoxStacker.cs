using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#define 
public class BoxStacker : MonoBehaviour
{
    [SerializeField]
    GameObject[] boxes = null;
    [SerializeField]
    Color[] colors;
    [SerializeField]
    GameObject spawnPoint = null;
    Vector3 positionTracker;
    float boxHeight = 0;

    [SerializeField]
    float timeBetweenEachBoxSpawned = 0.15f;

    List<GameObject> boxesSpawned;
    void Start()
    {
        positionTracker = spawnPoint.transform.position;
        RaycastHit hit;
        Physics.Raycast(positionTracker, Vector3.down, out hit, 2);
        boxHeight = boxes[0].GetComponent<MeshRenderer>().bounds.extents.y * 2;

        // add 1/2 height to it.
        positionTracker = hit.point;
        positionTracker.y += boxHeight / 2 +0.01f;
        boxesSpawned = new List<GameObject>();
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

        StartCoroutine("AddBoxTimed", numBoxes);
    }
    IEnumerator AddBoxTimed(int numBoxes)
    {
        for(int i=0; i<numBoxes; i++)
        {
            //Vector3 position = positionTracker;
            GameObject obj = Instantiate(boxes[0], positionTracker, spawnPoint.transform.rotation);
            positionTracker.y += boxHeight;
            boxesSpawned.Add(obj);
            yield return new WaitForSeconds(timeBetweenEachBoxSpawned);
        }
    }
   /* IEnumerator AnimateFrom()
    {
        var orderedButtons = OrderButtonsRightToLeft();
        foreach (var button in orderedButtons)// hide
        {
            Vector3 position = button.gameObject.transform.position;
            button.gameObject.SetActive(true);
            position.x += -600;
            iTween.MoveFrom(button.gameObject, position, 2);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
        scrollOutAllowed = true;
    }*/
}
