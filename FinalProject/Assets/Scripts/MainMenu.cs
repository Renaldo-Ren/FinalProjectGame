using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        //CombatTextManage.MyInstance.StartWriteText();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    //public void SetFullScreen (bool isFullScreen)
    //{
    //    Screen.fullScreen = isFullScreen;
    //}
}
