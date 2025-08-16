using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple main menu that displays buttons to start the game, open settings, or quit.
/// </summary>
public class MainMenu : MonoBehaviour
{
    void OnGUI()
    {
        const int buttonWidth = 200;
        const int buttonHeight = 40;
        int x = (Screen.width - buttonWidth) / 2;
        int y = Screen.height / 2 - buttonHeight;

        if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "Play"))
        {
            SceneManager.LoadScene("SampleScene");
        }

        if (GUI.Button(new Rect(x, y + buttonHeight + 10, buttonWidth, buttonHeight), "Settings"))
        {
            SceneManager.LoadScene("Settings");
        }

        if (GUI.Button(new Rect(x, y + 2 * (buttonHeight + 10), buttonWidth, buttonHeight), "Quit"))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
