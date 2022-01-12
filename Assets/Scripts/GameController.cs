using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject PrimaryPrefab;
    public GameObject SecondaryAreaPrefab;
    public GameObject[] SecondaryDropdownPrefabs;

    private GameObject PrimaryGameObject;
    private GameObject SecondaryGameObject;
    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScqvw5wsawccqz2offUGJODxYf0ZGkBX8r68vBTVQhcT9mQ7w/formResponse";

    public Button BackButton;
    public Button NextButton;

    public Dropdown PrimaryDropdown;
    public Dropdown SecondaryDropdown;

    public Text QuestionText;
    public Text QuestionCounter;

    public int questionsAnswered = 0;
    public string SelectedPrimary;
    public string SelectedSecondary;

    public QuestionList QuestionList;

    //Instantiate Primary DropDown Zone. Add a Listener to track changes made to Primary Dropdown. Update Question Text with first question.
    void Start()
    {
        QuestionList = GetComponent<QuestionList>();
        PrimaryGameObject = Instantiate(PrimaryPrefab, this.transform);
        PrimaryGameObject.transform.parent = this.transform;
        PrimaryDropdown = PrimaryGameObject.GetComponentInChildren<Dropdown>();
        PrimaryDropdown.onValueChanged.AddListener(delegate
        {
            PrimaryDropDownValueChanged(PrimaryDropdown);
        }
        );
        QuestionText.text = QuestionList.Questions[0];
    }

    //assign the Text from the selected DropDown option to the variable SelectedPrimary. Spawn the corrisponding SecondaryDropDownPrefab. Makes New SecondaryPrefab a child of Secondary Area
    public void PrimaryDropDownValueChanged(Dropdown change)
    {
        if(SecondaryGameObject != null)
        {
            Destroy(SecondaryGameObject);
        }
        SelectedPrimary = change.options[change.value].text;

        SecondaryGameObject = Instantiate(SecondaryDropdownPrefabs[change.value - 1], SecondaryAreaPrefab.transform);
        SecondaryGameObject.transform.parent = SecondaryAreaPrefab.transform;
        SecondaryDropdown = SecondaryGameObject.GetComponentInChildren<Dropdown>();
        SecondaryDropdown.onValueChanged.AddListener(delegate
        {
            SecondaryDropDownValueChanged(SecondaryDropdown);
        }
       );
    }

    public void SecondaryDropDownValueChanged(Dropdown change)
    {
        SelectedSecondary = change.options[change.value].text;
        NextButton.interactable = true;
    }

    IEnumerator PostAnswers(string Question, string PrimaryAnswer, string SecondaryAnswer)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.279219714", Question);
        form.AddField("entry.1436109404", PrimaryAnswer);
        form.AddField("entry.1033249407", SecondaryAnswer);
        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }

    public void NextQuestion()
    {
        StartCoroutine(PostAnswers(QuestionText.text, SelectedPrimary, SelectedSecondary));
        questionsAnswered = questionsAnswered + 1;
        if(questionsAnswered >= QuestionList.Questions.Count)
        {
            SceneManager.LoadScene(2);
        }
        QuestionCounter.text = questionsAnswered.ToString();

        if (questionsAnswered < QuestionList.Questions.Count)
        {
            QuestionText.text = QuestionList.Questions[questionsAnswered];



            Destroy(PrimaryGameObject);
            Destroy(SecondaryGameObject);

            PrimaryGameObject = Instantiate(PrimaryPrefab, this.transform);
            PrimaryGameObject.transform.parent = this.transform;
            PrimaryDropdown = PrimaryGameObject.GetComponentInChildren<Dropdown>();
            PrimaryDropdown.onValueChanged.AddListener(delegate
            {
                PrimaryDropDownValueChanged(PrimaryDropdown);
            }
            );

            NextButton.interactable = false;
        }
        else
        {
            
        }
    }

    public void PreviousQuestion()
    {
        questionsAnswered = questionsAnswered - 1;
        QuestionCounter.text = questionsAnswered.ToString();
        QuestionText.text = QuestionList.Questions[questionsAnswered];



        Destroy(PrimaryGameObject);
        Destroy(SecondaryGameObject);

        PrimaryGameObject = Instantiate(PrimaryPrefab, this.transform);
        PrimaryGameObject.transform.parent = this.transform;
        PrimaryDropdown = PrimaryGameObject.GetComponentInChildren<Dropdown>();
        PrimaryDropdown.onValueChanged.AddListener(delegate
        {
            PrimaryDropDownValueChanged(PrimaryDropdown);
        }
        );

        NextButton.interactable = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if(questionsAnswered > 0)
        {
            BackButton.interactable = true;
        }
        else
        {
            BackButton.interactable = false;
        }
    }
}
