using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad = "GameScene";

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
