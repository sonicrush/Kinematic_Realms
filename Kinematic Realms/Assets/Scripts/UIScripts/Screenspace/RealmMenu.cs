using UnityEngine;
using UnityEngine.SceneManagement;

public class RealmMenu : MonoBehaviour
{
    public string accelerationScene;
    public string velocityScene;
    public string displacementScene;

    public void MoveToAcceleScene()
    {
        SceneManager.LoadScene(accelerationScene);
    }

    public void MoveToVeloScene()
    {
        SceneManager.LoadScene(velocityScene);
    }

    public void MoveToDisScene()
    {
        SceneManager.LoadScene(displacementScene);
    }


}
