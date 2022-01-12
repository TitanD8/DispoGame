using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject[] MenuPages;
    public Button NextButton;
    public Button StartButton;
    public Button BackButton;
    public int PageCount = 0;

    public void NextPage()
    {
        MenuPages[PageCount].SetActive(false);
        PageCount += 1;
        MenuPages[PageCount].SetActive(true);
    }

    public void PreviousPage()
    {
        MenuPages[PageCount].SetActive(false);
        PageCount -= 1;
        MenuPages[PageCount].SetActive(true);
    }

    public void StartQuiz()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        if(PageCount >= MenuPages.Length-1)
        {
            NextButton.gameObject.SetActive(false);
        }
        else
        {
            NextButton.gameObject.SetActive(true);
        }
    }


}
