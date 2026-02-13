using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : BaseCar
{
    private const string EMISSION_VARIABLE = "_EmissionColor";

    private const int BRAKE_LIGHTS_MATERIAL_INDEX = 2;

    private const string ACCELERATE_INPUT = "Accelerate";
    private const string STEER_INPUT = "Steer";
    private const string BRAKE_INPUT = "Brake";

    private PlayerInput playerInput;
    private float acceleration;
    private float steer;
    private float brake;

    protected override void Awake()
    {
        base.Awake();
        parMotor = -parMotor;
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
}