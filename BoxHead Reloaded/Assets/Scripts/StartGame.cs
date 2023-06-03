using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Single); // Load GameScene
    }

    public void QuitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Feedback()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdimHszS5tCvvCZWsIpwyKqPyQVmg1LdCKNov8k8K40ye5zdQ/viewform");
    }
}
