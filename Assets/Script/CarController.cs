using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : BaseCar
{
    public static CarController Instance { get; private set; }
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
        Instance = this;
        parMotor = -parMotor;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!IsGamePlaying) return;
        acceleration = playerInput.actions[ACCELERATE_INPUT].ReadValue<float>();
        steer = playerInput.actions[STEER_INPUT].ReadValue<float>();
        brake = playerInput.actions[BRAKE_INPUT].ReadValue<float>();

        ToggleBrakeLights(brake);
    }

    private void FixedUpdate()
    {
        if (!IsGamePlaying) return;
        var speedKmH = rb.linearVelocity.magnitude * 3.6f;
        if (speedKmH < maxSpeed)
        {
            SetMotorTorque(parMotor * acceleration);
        }
        else
        {
            SetMotorTorque(0);
        }
        SetSteeringAngle(maxWheelRotation * steer);
        SetWheelsBrakeTorque(brakeForce * brake);
        SetVisualWheelsPositionAndRotation();
    }
}