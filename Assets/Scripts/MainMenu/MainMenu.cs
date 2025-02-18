using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Para manejar la imagen del fundido

public class MainMenu : MonoBehaviour
{
    public Image fadeImage; // Imagen negra que se usará para el fundido
    public float fadeSpeed = 1f; // Velocidad del fundido

    public void OnclickMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnclickPlay()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnclickExit()
    {
        Application.Quit();
    }
}