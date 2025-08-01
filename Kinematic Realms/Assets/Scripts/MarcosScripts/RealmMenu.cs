using UnityEngine;
using UnityEngine.SceneManagement;

public class RealmMenu : MonoBehaviour
{
    public string accelerationScene;
    public string velocityScene;
    public string displacementScene;

    public void MoveToAcceleScene(string accelerationScene)
    {
        SceneManager.LoadScene(accelerationScene);
    }

    public void MoveToVeloScene(string velocityScene)
    {
        SceneManager.LoadScene(velocityScene);
    }

    public void MoveToDisScene(string displacementScene)
    {
        SceneManager.LoadScene(displacementScene);
    }


}
