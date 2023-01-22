using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public string sceneName;
    [SerializeField]
    private string newScenePassword;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player.MyInstance.scenePassword = newScenePassword;
            //Player.MyInstance.InCombat = false;
            SceneManager.LoadScene(sceneName);
        }
    }
}
