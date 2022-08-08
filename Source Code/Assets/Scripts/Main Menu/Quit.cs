using UnityEngine;

public class Quit: MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exiting game."); // Outputs to console.
    }
}