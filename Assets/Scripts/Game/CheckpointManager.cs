using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointManager : MonoBehaviour
{
    public List<GameObject> checkpoints; // Lista de checkpoints en el mapa
    private HashSet<GameObject> checkpointsPasados = new HashSet<GameObject>(); // Checkpoints alcanzados
    private int vueltasCompletadas = 0; // Contador de vueltas
    public int vueltasObjetivo = 3; // Número total de vueltas

    // TextMeshPro para mostrar información en pantalla
    public TextMeshProUGUI textoVueltasFirstPerson;
    public TextMeshProUGUI textoVueltasThirdPerson;
    public TextMeshProUGUI textoTiemposFirstPerson;
    public TextMeshProUGUI textoTiemposThirdPerson;
    public TextMeshProUGUI textoTiempoActualFirstPerson;
    public TextMeshProUGUI textoTiempoActualThirdPerson;

    private float tiempoInicioVuelta; // Inicio de la vuelta actual
    private List<float> tiemposVueltas = new List<float>(); // Lista de tiempos de vuelta
    private bool carreraEnCurso = true; // Control de la carrera

    private void Start()
    {
        // Inicializar UI
        textoVueltasFirstPerson.text = "LAPS 0/" + vueltasObjetivo;
        textoVueltasThirdPerson.text = "LAPS 0/" + vueltasObjetivo;
        textoTiemposFirstPerson.text = "";
        textoTiemposThirdPerson.text = "";
        textoTiempoActualFirstPerson.text = "00:00.00";
        textoTiempoActualThirdPerson.text = "00:00.00";

        // Desactivar el primer checkpoint al inicio
        checkpoints[0].SetActive(false);

        // Iniciar el temporizador para la primera vuelta
        tiempoInicioVuelta = Time.time;
    }

    private void Update()
    {
        if (carreraEnCurso)
        {
            // Actualizar el tiempo actual en vivo
            float tiempoActual = Time.time - tiempoInicioVuelta;
            string tiempoFormateado = FormatoTiempo(tiempoActual);

            textoTiempoActualFirstPerson.text = tiempoFormateado;
            textoTiempoActualThirdPerson.text = tiempoFormateado;
        }
    }

    public void RegistrarPaso(GameObject checkpoint)
    {
        // Si el checkpoint ya ha sido pasado, no lo contamos
        if (checkpointsPasados.Contains(checkpoint)) return;

        // Agregar el checkpoint actual a los pasados
        checkpointsPasados.Add(checkpoint);
        Debug.Log("Checkpoint alcanzado: " + checkpoint.name);

        // Si ha pasado por todos los checkpoints
        if (checkpointsPasados.Count == checkpoints.Count - 1) // Pasó por todos menos el primero
        {
            // Ahora activamos el primer checkpoint
            checkpoints[0].SetActive(true);
        }

        // Si ha pasado por todos los checkpoints y está en el primer checkpoint activado
        if (checkpointsPasados.Count == checkpoints.Count && checkpoint == checkpoints[0])
        {
            // Contabilizamos una vuelta
            vueltasCompletadas++;

            // Guardar el tiempo de la vuelta
            GuardarTiempoVuelta();

            // Actualizar el texto de vueltas
            ActualizarTextoVueltas();
            ActualizarTextoTiempos();

            // Si ha completado el número de vueltas objetivo, termina la carrera
            if (vueltasCompletadas >= vueltasObjetivo)
            {
                Debug.Log("¡Carrera completada!");
                carreraEnCurso = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
            }
            else
            {
                // Reiniciar los checkpoints pasados y los demás objetos para la siguiente vuelta
                Debug.Log("Iniciando vuelta " + (vueltasCompletadas + 1));
                ResetearVuelta();
            }
        }
    }

    void GuardarTiempoVuelta()
    {
        // Calcular y almacenar el tiempo de la vuelta
        float tiempoVuelta = Time.time - tiempoInicioVuelta;
        tiemposVueltas.Add(tiempoVuelta);

        // Reiniciar el contador para la siguiente vuelta
        tiempoInicioVuelta = Time.time;
    }

    void ActualizarTextoVueltas()
    {
        string textoVueltas = "LAPS " + vueltasCompletadas + "/" + vueltasObjetivo;
        textoVueltasFirstPerson.text = textoVueltas;
        textoVueltasThirdPerson.text = textoVueltas;
    }

    void ActualizarTextoTiempos()
    {
        string textoTiempos = "";
        foreach (float tiempo in tiemposVueltas)
        {
            textoTiempos += FormatoTiempo(tiempo) + "\n";
        }

        textoTiemposFirstPerson.text = textoTiempos;
        textoTiemposThirdPerson.text = textoTiempos;
    }

    string FormatoTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        int milisegundos = Mathf.FloorToInt((tiempo - Mathf.Floor(tiempo)) * 100);
        return string.Format("{0:00}:{1:00}.{2:00}", minutos, segundos, milisegundos);
    }

    void ResetearVuelta()
    {
        // Reiniciar la lista de checkpoints pasados
        checkpointsPasados.Clear();

        // Desactivar el primer checkpoint para la siguiente vuelta
        checkpoints[0].SetActive(false);
    }
}
