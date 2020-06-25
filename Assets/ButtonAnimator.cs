using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonAnimator : MonoBehaviour
{
    Transform[] buttonLocations;

    [SerializeField]
    Button[] buttons = null;
    bool scrollInNeeded = true;
    void Start()
    {
        HideAllButtons();
        SaveInitialLocations();
    }

    void HideAllButtons()
    {
        foreach (var button in buttons)// hide
        {
            button.gameObject.SetActive(false);
        }
    }

    void SaveInitialLocations()
    {
        buttonLocations = new Transform[buttons.Length];
        int index = 0;
        foreach (var button in buttons)// hide
        {
            buttonLocations[index++] = button.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(scrollInNeeded)
        {
            HideAllButtons();
            scrollInNeeded = false;
            StartCoroutine("AnimateFrom");
        }
    }

    List <Button> OrderButtonsRightToLeft()
    {
        return buttons.OrderByDescending(x => x.gameObject.transform.position.x).ToList();
    }

    IEnumerator AnimateFrom()
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
    }
}
