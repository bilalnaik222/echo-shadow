using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    // Call this method from a button OnClick() or elsewhere to return to Main Menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your actual scene name if different
    }
}
