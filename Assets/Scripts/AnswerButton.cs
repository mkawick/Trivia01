using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    bool isSelected = false;
    Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        defaultColor = GetComponent<Button>().colors.normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick()
    {
        Debug.Log("clicked");

        ColorBlock colors = GetComponent<Button>().colors;
        isSelected = !isSelected;
        if(isSelected == true)
            colors.normalColor = Color.green;
        else
            colors.normalColor = defaultColor;
        colors.highlightedColor = colors.normalColor;
        colors.selectedColor = colors.normalColor;

        GetComponent<Button>().colors = colors;

        Debug.Log("You have clicked the button!");
    }
}
