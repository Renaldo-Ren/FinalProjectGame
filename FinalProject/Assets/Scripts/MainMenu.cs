using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Destroy(GameObject.Find("MainParent"));
        //CombatTextManage.MyInstance.StartWriteText();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void WinToMain()
    {
        SceneManager.LoadScene("Main Menu");
        //Player.MyInstance.ResetPlayer();
    }

    //public void SetFullScreen (bool isFullScreen)
    //{
    //    Screen.fullScreen = isFullScreen;
    //}
}
