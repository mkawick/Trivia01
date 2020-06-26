using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#define 
public class BoxStacker : MonoBehaviour
{
    [SerializeField]
    GameObject[] boxes;
    [SerializeField]
    Color[] colors;
    [SerializeField]
    GameObject spawnPoint;
    Vector3 positionTracker;
    float boxHeight = 0;
    void Start()
    {
        positionTracker = spawnPoint.transform.position;
        boxHeight = boxes[0].GetComponent<MeshRenderer>().bounds.extents.y*2;
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
            yield return new WaitForSeconds(0.15f);
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
