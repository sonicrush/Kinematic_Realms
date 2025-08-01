using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour { 
    public void MoveToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication() {
        Application.Quit();
        Debug.Log("Application has been exited successfully.");
    }
}
