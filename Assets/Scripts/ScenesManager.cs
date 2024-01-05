using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameObject menuCanvas; // Drag your menu GameObject here in the Unity Editor

    void Start()
    {
        // Optionally, you can deactivate the menu at the start of the scene
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
    }

    public void OnPlayEasy()
    {
        SceneManager.LoadScene("Level_Easy");
        Debug.Log("scene is loading");

        // Activate the menu after loading the scene
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
    }

    public void OnPlayHard()
    {
        SceneManager.LoadScene("Level_Timer");
    }

    public void OnInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
