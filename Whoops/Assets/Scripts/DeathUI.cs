using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public void NextLevel()
    {
        FindObjectOfType<AudioManager>().Play("Press");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu(int index)
    {
        FindObjectOfType<AudioManager>().Play("Press");
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    } 

    public void Restart()
    {
        FindObjectOfType<AudioManager>().Play("Press");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
