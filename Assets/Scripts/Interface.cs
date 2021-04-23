using UnityEngine;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public void runGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void quiteGame()
    {
        Application.Quit();
    }
}