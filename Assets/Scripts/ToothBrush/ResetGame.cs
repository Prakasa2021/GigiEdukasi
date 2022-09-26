using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public string _path;
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_path);
    }
}
