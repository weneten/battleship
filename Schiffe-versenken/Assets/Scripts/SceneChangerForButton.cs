using UnityEngine;
using UnityEngine.SceneManagement; // Important for scene management
using UnityEngine.UI; // For interacting with UI elements (like Button)

public class SceneLoader : MonoBehaviour
{
    // Public variable to set the name of the scene to load in the Inspector
    public string sceneToLoad;

    // This function will be called when the button is clicked
    public void LoadTargetScene()
    {
        // Ensure the scene name is not empty
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name is not set in the Inspector for SceneLoader!");
        }
    }
}