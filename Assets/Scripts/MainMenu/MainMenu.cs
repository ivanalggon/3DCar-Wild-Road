using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Para manejar la imagen del fundido

public class MainMenu : MonoBehaviour
{
    // Audio de fondo del menú principal
    public AudioSource audioSource;
    public AudioClip audioClip;

    public void Start()
    {
        Time.timeScale = 1f;

        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void OnclickMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnclickPlay()
    {
        SceneManager.LoadScene("Level_1");
        audioSource.Stop();
    }

    public void OnclickExit()
    {
        audioSource.Stop();
        Application.Quit();
    }
}