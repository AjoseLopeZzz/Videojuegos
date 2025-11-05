using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Puedes asignar esta variable desde el Inspector o por código
    [SerializeField] private string sceneName;

    // Método público para cambiar de escena
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No se ha asignado un nombre de escena.");
        }
    }

    // Método alternativo para pasar el nombre directamente
    public void LoadSceneByName(string newSceneName)
    {
        if (!string.IsNullOrEmpty(newSceneName))
        {
            SceneManager.LoadScene(newSceneName);
        }
        else
        {
            Debug.LogError("El nombre de la escena pasada está vacío o es nulo.");
        }
    }
}
