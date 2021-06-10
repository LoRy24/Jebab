using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAreaCollider : MonoBehaviour
{
    [SerializeField] LevelScript levelScript;
    [SerializeField] GameObject wonUI;
    
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0f;
        levelScript.isEnd = true;
        wonUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
