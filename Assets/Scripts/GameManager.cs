using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    TMP_Text text;
    public Button questionButton;
    public Button[] buttons;
    public Color newColor;
    private Dictionary<int, TriviaQuestion> questionMap;

    private void Awake()
    {
        questionMap = new Dictionary<int, TriviaQuestion>();
        var questions = Resources.FindObjectsOfTypeAll<TriviaQuestion>();
        for(int i=0; i< questions.Length; i++)
        {
            questionMap.Add(questions[i].GetInstanceID(), questions[i]);
        }

        /*TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
        textmeshPro.color = new Color32(255, 128, 0, 255);*/
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
