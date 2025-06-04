using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame () 
    {
        SceneManager.LoadScene("Game");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ExitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
