using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private const string EMISSION_VARIABLE = "_EmissionColor";

    private const int BRAKE_LIGHTS_MATERIAL_INDEX = 2;

    private const string ACCELERATE_INPUT = "Accelerate";
    private const string STEER_INPUT = "Steer";
    private const string BRAKE_INPUT = "Brake";
    [SerializeField] private WheelCollider wheelFR, wheelFL, wheelBR, wheelBL;
    private Transform wheelFRVisual, wheelFLVisual, wheelBRVisual, wheelBLVisual;
    [SerializeField] private float parMotor;
    [SerializeField] private float maxWheelRotation;
    [SerializeField] private float brakeForce;
    private MeshRenderer meshRenderer;

    private PlayerInput playerInput;
    private float acceleration;
    private float steer;
    private float brake;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        parMotor = -parMotor;
        SetWheelVisuals(wheelFR, ref wheelFRVisual);
        SetWheelVisuals(wheelFL, ref wheelFLVisual);
        SetWheelVisuals(wheelBR, ref wheelBRVisual);
        SetWheelVisuals(wheelBL, ref wheelBLVisual);
    }

    private void SetWheelVisuals(WheelCollider wheelCollider, ref Transform wheelVisual)
    {
        wheelVisual = wheelCollider.transform.GetChild(0);
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        acceleration = playerInput.actions[ACCELERATE_INPUT].ReadValue<float>();
        steer = playerInput.actions[STEER_INPUT].ReadValue<float>();
        brake = playerInput.actions[BRAKE_INPUT].ReadValue<float>();

        var brakeLightMaterial = meshRenderer.materials[BRAKE_LIGHTS_MATERIAL_INDEX];
        if (brake > 0)
        {
            brakeLightMaterial.SetColor(EMISSION_VARIABLE, Color.red);
        }
        else
        {
            brakeLightMaterial.SetColor(EMISSION_VARIABLE, Color.black);
        }
    }

    private void FixedUpdate()
    {
        wheelBL.motorTorque = parMotor * acceleration;
        wheelBR.motorTorque = parMotor * acceleration;

        wheelFL.steerAngle = maxWheelRotation * steer;
        wheelFR.steerAngle = maxWheelRotation * steer;

        wheelFR.brakeTorque = brakeForce * brake;
        wheelFL.brakeTorque = brakeForce * brake;
        wheelBR.brakeTorque = brakeForce * brake;
        wheelBL.brakeTorque = brakeForce * brake;

        SetWheelPositionAndRotation(wheelBR, wheelBRVisual);
        SetWheelPositionAndRotation(wheelBL, wheelBLVisual);
        SetWheelPositionAndRotation(wheelFR, wheelFRVisual);
        SetWheelPositionAndRotation(wheelFL, wheelFLVisual);
    }

    private void SetWheelPositionAndRotation(WheelCollider wheelCollider, Transform wheelVisual)
    {
        wheelCollider.GetWorldPose(out var wheelPosition, out var wheelRotation);
        wheelVisual.transform.position = wheelPosition;
        wheelVisual.transform.rotation = wheelRotation;
    }
}