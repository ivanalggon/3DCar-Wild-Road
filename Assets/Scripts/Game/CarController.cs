using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    // Efecto Sangre
    public GameObject bloodEffect;
    // Ruedas delanteras
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    
    // Ruedas traseras
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    // Transform para la visualizacion de las ruedas
    public Transform frontLeftWheelMesh;
    public Transform frontRightWheelMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;

    public TMP_Text textoVelocidadActual1;
    public TMP_Text textoVelocidadActual2;

    // camara del coche
    public Camera thirdPerson;
    public Camera firstPerson;

    public AudioListener audioThirdPerson;
    public AudioListener audioFirstPerson;

    // Variables para el control del coche
    public float motorForce = 1000f; // Fuerza de motor
    public float brakeForce = 1000f; // Fuerza de frenado
    public float maxSteerAngle = 30f; // Angulo maximo de giro

    private float currentBrakeForce = 0f; // Fuerza de frenado actual
    private float currentSteerAngle = 0f; // Angulo de giro actual

    private float currentSpeed = 0f; // Velocidad actual
    private float maxSpeed = 120f; // Velocidad maxima

    public void Start()
    {
        // Inicializar la camara
        thirdPerson.enabled = true;
        firstPerson.enabled = false;

        // Inicializar el audio
        audioThirdPerson.enabled = true;
        audioFirstPerson.enabled = false;

        // Activar el texto de la velocidad actual 1
        textoVelocidadActual1.enabled = true;
        textoVelocidadActual2.enabled = false;
    }
    void Update()
    {
        // Obtener la velocidad del Rigidbody
        Vector3 velocidad = GetComponent<Rigidbody>().velocity;

        // Convertir la velocidad a km/h
        currentSpeed = velocidad.magnitude * 3.6f;

        // Mostrar la velocidad actual
        textoVelocidadActual1.text = currentSpeed.ToString("0") + " km/h";
        textoVelocidadActual2.text = currentSpeed.ToString("0") + " km/h";

        // Cambiar la camara
        CambiarCamara();
    }

    public void CambiarCamara()
    {
        // Cambiar la camara pulsando C
        if (Input.GetKeyDown(KeyCode.C))
        {
            thirdPerson.enabled = !thirdPerson.enabled;
            firstPerson.enabled = !firstPerson.enabled;

            // Cambiar el audio
            audioThirdPerson.enabled = !audioThirdPerson.enabled;
            audioFirstPerson.enabled = !audioFirstPerson.enabled;

            // Cambiar el texto de la velocidad actual
            textoVelocidadActual1.enabled = !textoVelocidadActual1.enabled;
            textoVelocidadActual2.enabled = !textoVelocidadActual2.enabled;
        }
    }
    private void FixedUpdate()
    {
        //Recoger entrada de usuario
        float moveInput = Input.GetAxis("Vertical"); // acelerar o frenar (W,S)
        float steerInput = Input.GetAxis("Horizontal"); // girar (A,D)
        bool isBraking = Input.GetKey(KeyCode.Space); // freno (barra espaciadora)

        //Control del vehiculo
        HandleMotor(moveInput, isBraking);
        HandleSteering(steerInput);
        UpdateWheels();
    }

    void HandleMotor(float moveInput, bool isBraking)
    {
        // Aplicar torque solo si la velocidad es menor a maxSpeed
        if (currentSpeed < maxSpeed || moveInput < 0)
        {
            rearLeftWheel.motorTorque = moveInput * motorForce;
            rearRightWheel.motorTorque = moveInput * motorForce;
        }
        else
        {
            // Si supera maxSpeed, evitar seguir acelerando
            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
        }

        //Aplicar freno
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBrake();
    }
    void ApplyBrake()
    {
        //Frenar todas las ruedas
        frontLeftWheel.brakeTorque = currentBrakeForce;
        frontRightWheel.brakeTorque = currentBrakeForce;
        rearLeftWheel.brakeTorque = currentBrakeForce;
        rearRightWheel.brakeTorque = currentBrakeForce;
    }
    void HandleSteering(float steerInput)
    {
        // Calcular el angulo de giro
        currentSteerAngle = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }
    void UpdateWheels()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftWheelMesh);
        UpdateWheelPose(frontRightWheel, frontRightWheelMesh);
        UpdateWheelPose(rearLeftWheel, rearLeftWheelMesh);
        UpdateWheelPose(rearRightWheel, rearRightWheelMesh);
    }
    void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        // Posicion y rotacion de la rueda
        Vector3 pos;
        Quaternion rot;

        // Posicion y rotacion de la rueda del collider
        collider.GetWorldPose(out pos, out rot);

        // Aplicar la posicion y rotacion a la rueda
        transform.position = pos;
        transform.rotation = rot;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            other.gameObject.SetActive(false);
            Vector3 bloodPosition = other.transform.position + Vector3.up * 1.0f;
            Instantiate(bloodEffect, bloodPosition, Quaternion.identity);

            CheckpointManager manager = FindObjectOfType<CheckpointManager>();
            // sumar 5 segundos al tiempo de la vuelta actual
            manager.tiempoInicioVuelta -= 5f;

        }
        else if (other.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            CheckpointManager manager = FindObjectOfType<CheckpointManager>();
            manager.tiempoInicioVuelta += 1f;
        }
    }
}
