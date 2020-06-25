using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonAnimator : MonoBehaviour
{
    Vector3[] buttonLocations;

    [SerializeField]
    Button[] buttons = null;
    bool scrollInNeeded = true;
    bool scrollOutAllowed = false;
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
        buttonLocations = new Vector3[buttons.Length];
        int index = 0;
        foreach (var button in buttons)// hide
        {
            buttonLocations[index++] = button.transform.position;
        }
    }
    void RestoreButtonsInInitialLocations()
    {
        int index = 0;
        foreach (var button in buttons)// hide
        {
            button.transform.position = buttonLocations[index++];
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
        else if(scrollOutAllowed == true)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                StartCoroutine("AnimateOut");
            }
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
        scrollOutAllowed = true;
    }
    IEnumerator AnimateOut()
    {
        var orderedButtons = OrderButtonsRightToLeft();
        foreach (var button in orderedButtons)// hide
        {
            Vector3 position = button.gameObject.transform.position;
            button.gameObject.SetActive(true);
            position.x += 600;
            iTween.MoveTo(button.gameObject, position, 2);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
        yield return new WaitForSeconds(2.0f);
        scrollOutAllowed = false;
        RestoreButtonsInInitialLocations();
    }

    
}
