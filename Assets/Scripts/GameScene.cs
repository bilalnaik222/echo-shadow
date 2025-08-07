using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public string sceneToLoad = "GameScene";

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
