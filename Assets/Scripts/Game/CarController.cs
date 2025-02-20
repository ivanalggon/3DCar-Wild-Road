using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Efectos
    public GameObject bloodEffect;
    public GameObject diamondEffect;

    // Ruedas delanteras
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    
    // Ruedas traseras
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    // Transform para la visualización de las ruedas
    public Transform frontLeftWheelMesh;
    public Transform frontRightWheelMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;

    // Texto para la velocidad actual
    public TMP_Text textoVelocidadActual;

    // Audio del coche
    public AudioSource motorAudio;
    public AudioClip motorClip;

    // Cámaras
    public Camera thirdPerson;
    public Camera firstPerson;

    // Variables para el control del coche
    public float motorForce = 1000f; // Fuerza de motor
    public float brakeForce = 1000f; // Fuerza de frenado
    public float maxSpeed = 120f; // Velocidad máxima

    private float currentSpeed = 0f; // Velocidad actual
    private bool isOnRoad = true; // Variable para saber si el coche está en la carretera
    private CheckpointManager manager;

    private void Start()
    {
        // Configurar el AudioSource
        motorAudio.clip = motorClip;
        motorAudio.loop = true;
        motorAudio.playOnAwake = false;
        motorAudio.volume = 0.3f; // Volumen inicial
        motorAudio.Play();
        
        manager = FindObjectOfType<CheckpointManager>();

        // Inicializar la cámara
        thirdPerson.enabled = true;
        firstPerson.enabled = false;

        // Activar el texto de la velocidad actual
        textoVelocidadActual.enabled = true;
    }

    private void Update()
    {
        // Obtener la velocidad del Rigidbody
        Vector3 velocidad = GetComponent<Rigidbody>().velocity;
        currentSpeed = velocidad.magnitude * 3.6f; // Convertir la velocidad a km/h

        // Mostrar la velocidad actual
        textoVelocidadActual.text = currentSpeed.ToString("0") + " km/h";

        // Ajustar la fuerza del motor dependiendo de si está sobre la carretera o no
        isOnRoad = IsWheelOnRoad(frontLeftWheel) || IsWheelOnRoad(frontRightWheel) || 
                   IsWheelOnRoad(rearLeftWheel) || IsWheelOnRoad(rearRightWheel);
        
        motorForce = isOnRoad ? 1000f : 200f;  // Si está fuera de la carretera, reduce la fuerza a la mitad

        // Ajustar el sonido del motor según la velocidad
        AjustarSonidoMotor();

        // Cambiar la cámara si es necesario
        CambiarCamara();
    }

    bool IsWheelOnRoad(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            // Verificar si el collider tocado tiene el tag "Road"
            return hit.collider.CompareTag("Road");
        }
        return false; // La rueda no está tocando la carretera
    }

    private void AjustarSonidoMotor()
    {
        if (!motorAudio) return; // Asegurar que el AudioSource existe

        // Normalizar el pitch dentro de un rango realista
        float minPitch = 0.5f;
        float maxPitch = 2.0f;

        // Ajustar el volumen y el pitch según la velocidad
        motorAudio.pitch = Mathf.Lerp(minPitch, maxPitch, currentSpeed / maxSpeed);
        motorAudio.volume = Mathf.Lerp(0.2f, 1.0f, currentSpeed / maxSpeed);

        // Asegurar que el sonido del motor se reproduce solo cuando el coche se mueve
        if (currentSpeed > 0.5f && !motorAudio.isPlaying)
        {
            motorAudio.Play();
        }
        else if (currentSpeed <= 0.5f && motorAudio.isPlaying)
        {
            motorAudio.Stop();
        }
    }

    public void CambiarCamara()
    {
        // Cambiar la cámara pulsando C
        if (Input.GetKeyDown(KeyCode.C))
        {
            thirdPerson.enabled = !thirdPerson.enabled;
            firstPerson.enabled = !firstPerson.enabled;
        }
    }

    private void FixedUpdate()
    {
        // Recoger entrada de usuario
        float moveInput = Input.GetAxis("Vertical"); // Acelerar o frenar (W, S)
        float steerInput = Input.GetAxis("Horizontal"); // Girar (A, D)
        bool isBraking = Input.GetKey(KeyCode.Space); // Freno (barra espaciadora)

        // Control del vehículo
        HandleMotor(moveInput, isBraking);
        HandleSteering(steerInput);
        UpdateWheels();
    }

    void HandleMotor(float moveInput, bool isBraking)
    {
        // Si el coche está en la carretera, aplicar la fuerza normal
        float motorMultiplier = isOnRoad ? 1f : 0.2f;

        // Aplicar torque solo si la velocidad es menor a maxSpeed
        if (currentSpeed < maxSpeed || moveInput < 0)
        {
            rearLeftWheel.motorTorque = moveInput * motorForce * motorMultiplier;
            rearRightWheel.motorTorque = moveInput * motorForce * motorMultiplier;
        }
        else
        {
            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
        }

        // Aplicar frenos
        ApplyBrake(isBraking);
    }

    void ApplyBrake(bool isBraking)
    {
        // Aplicar la fuerza de freno a todas las ruedas
        float currentBrakeForce = isBraking ? brakeForce : 0f;
        frontLeftWheel.brakeTorque = currentBrakeForce;
        frontRightWheel.brakeTorque = currentBrakeForce;
        rearLeftWheel.brakeTorque = currentBrakeForce;
        rearRightWheel.brakeTorque = currentBrakeForce;
    }

    void HandleSteering(float steerInput)
    {
        // Calcular el ángulo de giro
        float currentSteerAngle = steerInput * 30f; // Usamos 30f como el ángulo máximo de giro
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }

    void UpdateWheels()
    {
        // Actualizar la posición y rotación de cada rueda
        UpdateWheelPose(frontLeftWheel, frontLeftWheelMesh);
        UpdateWheelPose(frontRightWheel, frontRightWheelMesh);
        UpdateWheelPose(rearLeftWheel, rearLeftWheelMesh);
        UpdateWheelPose(rearRightWheel, rearRightWheelMesh);
    }

    void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        // Posición y rotación de la rueda
        Vector3 pos;
        Quaternion rot;

        collider.GetWorldPose(out pos, out rot);

        transform.position = pos;
        transform.rotation = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            other.gameObject.SetActive(false);
            Vector3 bloodPosition = other.transform.position + Vector3.up * 1.0f;
            GameObject blood = Instantiate(bloodEffect, bloodPosition, Quaternion.identity);
            Destroy(blood, 3f); // Destruir la partícula después de 3 segundos

            manager.tiempoInicioVuelta -= 5f;
        }
        else if (other.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            Vector3 diamondPosition = other.transform.position + Vector3.up * 1.0f;
            GameObject diamond = Instantiate(diamondEffect, diamondPosition, Quaternion.identity);
            Destroy(diamond, 3f); // Destruir la partícula después de 3 segundos

            manager.tiempoInicioVuelta += 1f;
        }
    }
}
