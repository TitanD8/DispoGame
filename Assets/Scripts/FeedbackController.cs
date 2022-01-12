using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FeedbackController : MonoBehaviour
{
    public GameObject[] FeedbackPages;

    public Button NextButton;
    public Button SubmitButton;

    public InputField nameInput;
    public InputField feedbackInput;

    public Slider Guide;
    public Slider Scenario;
    public Slider Quiz;

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfkMkUVhn2tYS54oxITvMWf5QkwE9oe-_WkRxf-zg5QtUb3pw/formResponse";
    public string Name;
    public string Feedback1;
    public string Feedback2;
    public string Feedback3;
    public string Feedback4;

    public int Counter = 0;

    public void NextPage()
    {
        if(Counter == 1)
        {
            NextButton.gameObject.SetActive(false);
            SubmitButton.gameObject.SetActive(true);
        }
        FeedbackPages[Counter].SetActive(false);
        Counter += 1;
        FeedbackPages[Counter].SetActive(true);
    }

    public void SubmitName()
    {
        Name = nameInput.text;
        NextButton.interactable = true;
    }

    public void GuideSlider()
    {
        Feedback1 = Guide.value.ToString();
    }
    
    public void ScenarioSlider()
    {
        Feedback2 = Scenario.value.ToString();
    }
    
    public void QuizSlider()
    {
        Feedback3 = Quiz.value.ToString();
    }

    public void OtherFeedback()
    {
        Feedback4 = feedbackInput.text;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator SubmitAnswers(string name, string guide, string scenarios, string quiz, string other)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.2039603430", Name);
        form.AddField("entry.1566368995", guide);
        form.AddField("entry.385562972", scenarios);
        form.AddField("entry.1169339026", quiz);
        form.AddField("entry.992464088", other);
        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }

    public void SubmitQuiz()
    {
        StartCoroutine(SubmitAnswers(Name, Feedback1, Feedback2, Feedback3, Feedback4));
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
