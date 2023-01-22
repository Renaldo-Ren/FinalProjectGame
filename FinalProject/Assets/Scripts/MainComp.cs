using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainComp : MonoBehaviour
{
    private static MainComp instance;
    public static MainComp MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MainComp>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if((SceneManager.GetActiveScene().buildIndex == 0) || (SceneManager.GetActiveScene().buildIndex == 6))
        {
            Destroy(gameObject);
        }
    }
}
