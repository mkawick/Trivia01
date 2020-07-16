using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    internal bool isSelected = false;
    Color defaultColor;
    internal Color highlightColor;
    public Color correctAnswerHighlightColor;
    public Color incorrectAnswerHighlightColor;
    Sprite defaultButtonSprite = null;
    public bool isCorrectAnswer = false;

    [SerializeField]
    GameObject colorOverlay = null;
    internal string Answer 
    { 
        get { return GetComponentInChildren<TMPro.TextMeshProUGUI>().text; } 
        set { GetComponentInChildren<TMPro.TextMeshProUGUI>().text = value; } 
    }
    internal Sprite pic
    {
        //Image img = this.GetComponent<Image>();
        get { return GetComponent<Image>().sprite; }
        set 
        {
            if(value == null)
            {
                this.GetComponent<Image>().sprite = defaultButtonSprite;
            }
            else
            {
                this.GetComponent<Image>().sprite = value;
                GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            }
        }
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        defaultColor = GetComponent<Button>().colors.normalColor;
        Debug.Assert(colorOverlay != null);
        defaultButtonSprite = this.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void TaskOnClick()
    {
        ColorBlock colors = GetComponent<Button>().colors;
        isSelected = !isSelected;
        if(isSelected == true)
            colors.normalColor = highlightColor;
        else
            colors.normalColor = defaultColor;
        colors.highlightedColor = colors.normalColor;
        colors.selectedColor = colors.normalColor;

        GetComponent<Button>().colors = colors;
    }

    public void ShowCorrectOverlay()
    {
        ResetColors();
        colorOverlay.SetActive(true);
        colorOverlay.GetComponent<Image>().color = correctAnswerHighlightColor;
    }
    public void ShowIncorrectOverlay()
    {
        ResetColors();
        colorOverlay.SetActive(true);
        colorOverlay.GetComponent<Image>().color = incorrectAnswerHighlightColor;
    }

    void ResetColors()
    {
        ColorBlock colors = GetComponent<Button>().colors;
        colors.normalColor = defaultColor;
        colors.highlightedColor = colors.normalColor;
        colors.selectedColor = colors.normalColor;
        GetComponent<Button>().colors = colors;
    }
    public void Reset()
    {
        isSelected = false;
        ResetColors();
        colorOverlay.SetActive(false);
    }
}
