using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonidos : MonoBehaviour
{
    public AudioClip animalSound;
    public AudioClip diamondSound;
    public AudioClip frenoSonido;
    public AudioClip sonidoAmbiente;

    public AudioListener ListennerThirdPerson;
    public AudioListener ListennerFirstPerson;

    private AudioSource audioSource; // AudioSource para reproducir sonidos

    private void Start()
    {
        // Crear o buscar un AudioSource en este GameObject
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el AudioSource
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.clip = sonidoAmbiente;
        audioSource.Play(); // Reproducir el sonido ambiente en bucle

        // Inicializar el audio
        ListennerThirdPerson.enabled = true;
        ListennerFirstPerson.enabled = false;
    }

    public void CambiarAudio()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Cambiar el AudioListener activo
            ListennerThirdPerson.enabled = !ListennerThirdPerson.enabled;
            ListennerFirstPerson.enabled = !ListennerFirstPerson.enabled;
        }
    }

    public void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Reproducir sonido sin cortar el sonido ambiente
        }
    }

    private void Update()
    {
        CambiarAudio();
        Frenar();
    }

    // Reproducir sonido de freno al presionar la tecla de freno
    public void Frenar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReproducirSonido(frenoSonido);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            ReproducirSonido(animalSound);
        }
        else if (other.CompareTag("Diamond"))
        {
            ReproducirSonido(diamondSound);
        }
    }
}
