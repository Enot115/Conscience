using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{


    public void Start()
    {
        //SceneManager.LoadScene("level1");
    }

    public void ExitGame()
    { 
        Application.Quit();
    }

    public void LoadScene() 
    {
        SceneManager.LoadScene("level1");
    }
}
