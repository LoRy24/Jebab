using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private int currentLevel;

    public void RestartCurrent()
    {
        SceneManager.LoadScene(currentLevel);
        Debug.Log("Level restarted!");
    }

    public void LoadScene(int s)
    {
        SceneManager.LoadScene(s);
        Debug.Log("Loaded scene " + s);
    }

    public void setTimeSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
