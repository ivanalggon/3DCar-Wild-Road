using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Referencia al Canvas del men� de pausa
    public GameObject pauseMenuCanvas;

    // Bandera para saber si el juego est� pausado
    private bool isPaused = false;

    void Start()
    {
        // Asegurarse de que el Canvas de pausa est� desactivado al principio
        pauseMenuCanvas.SetActive(false);
    }

    void Update()
    {
        // Comprobar si se presion� el bot�n Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pausar el juego
    void PauseGame()
    {
        isPaused = true;

        // Activar el Canvas de pausa
        pauseMenuCanvas.SetActive(true);

        // Detener el tiempo (Game Paused)
        Time.timeScale = 0f;

        // Pausar audio
        AudioListener.pause = true;
    }

    // Reanudar el juego
    void ResumeGame()
    {
        isPaused = false;

        // Desactivar el Canvas de pausa
        pauseMenuCanvas.SetActive(false);

        // Reanudar el tiempo
        Time.timeScale = 1f;

        // Reanudar audio
        AudioListener.pause = false;
    }

    public void OnclickMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnclickExit()
    {
        Application.Quit();
    }
}