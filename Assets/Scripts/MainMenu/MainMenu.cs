using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Para manejar la imagen del fundido

public class MainMenu : MonoBehaviour
{
    public Image fadeImage; // Imagen negra que se usará para el fundido
    public float fadeSpeed = 1f; // Velocidad del fundido

    private void Start()
    {
        fadeImage.gameObject.SetActive(false); // Asegurarse de que el fundido no se vea al principio
    }

    public void OnclickMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnclickPlay()
    {
        StartCoroutine(TransitionToGame());
    }

    public void OnclickExit()
    {
        Application.Quit();
    }

    private IEnumerator TransitionToGame()
    {
        // Activar la imagen de fundido y hacerla visible
        fadeImage.gameObject.SetActive(true);

        // Hacer el fundido a negro
        yield return FadeIn();

        // Cambiar de escena
        SceneManager.LoadScene("Level_1");

        // Hacer el fundido desde negro
        yield return FadeOut();
    }

    private IEnumerator FadeIn()
    {
        Color currentColor = fadeImage.color;
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(0, 1, time));
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        Color currentColor = fadeImage.color;
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(1, 0, time));
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // Desactivar la imagen después del fundido
    }
}
