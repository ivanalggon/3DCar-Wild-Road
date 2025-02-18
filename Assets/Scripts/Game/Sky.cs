using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    public Transform followTarget;  // El objeto a seguir (el coche)
    public GameObject starPrefab;   // Prefab de estrella (quad)
    public int numberStars = 100;   // Número de estrellas
    public float skyRadius = 500f;  // Radio del cielo

    private void Start()
    {
        GenerateSky();
    }

    void LateUpdate()
    {
        // Mantener el cielo centrado en el coche
        transform.position = followTarget.position;
    }

    void GenerateSky()
    {
        for (int i = 0; i < numberStars; i++)
        {
            // Generar una posición aleatoria en una esfera alrededor del jugador
            Vector3 pos = Random.onUnitSphere * skyRadius;
            pos += followTarget.position; // Centrar las estrellas en la posición inicial del coche

            // Instanciar la estrella
            GameObject star = Instantiate(starPrefab, pos, Quaternion.identity, transform);

            // Hacer que la estrella mire HACIA el jugador pero corrigiendo el eje
            star.transform.LookAt(followTarget.position, Vector3.up);

            // Ajustar la orientación para que la cara del quad sea visible
            star.transform.Rotate(0, 180, 0); // Gira 180° para que la cara del quad mire al jugador

            // Ajustar el tamaño si es necesario
            star.transform.localScale = Vector3.one * 2f; // Ajusta el tamaño según necesidad
        }
    }
}
