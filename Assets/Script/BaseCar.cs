using System;
using UnityEngine;

public class BaseCar : MonoBehaviour
{
    private const string EMISSION_VARIABLE = "_EmissionColor";
    private const int BRAKE_LIGHTS_MATERIAL_INDEX = 2;
    
    [SerializeField] protected WheelCollider wheelFR, wheelFL, wheelBR, wheelBL;
    protected Transform wheelFRVisual, wheelFLVisual, wheelBRVisual, wheelBLVisual;
    [SerializeField] protected float parMotor;
    [SerializeField] protected float maxWheelRotation;
    [SerializeField] protected float brakeForce;
    protected MeshRenderer meshRenderer;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        SetWheelVisuals(wheelFR, ref wheelFRVisual);
        SetWheelVisuals(wheelFL, ref wheelFLVisual);
        SetWheelVisuals(wheelBR, ref wheelBRVisual);
        SetWheelVisuals(wheelBL, ref wheelBLVisual);
    }

    protected void SetSteeringAngleFromDirection(Vector3 direction)
    {
        var rotation = Quaternion.FromToRotation(transform.forward, direction);

        SetSteeringAngle(rotation.eulerAngles.y);
    }
    
    protected void SetSteeringAngle(float angle)
    {
        wheelFR.steerAngle = angle;
        wheelFL.steerAngle = angle;
    }

    protected void SetMotorTorque(float torque)
    {
        wheelBL.motorTorque = torque;
        wheelBR.motorTorque = torque;
    }
    
    protected void ToggleBrakeLights(float brake)
    {
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


    private void SetWheelVisuals(WheelCollider wheelCollider, ref Transform wheelVisual)
    {
        wheelVisual = wheelCollider.transform.GetChild(0);
    }

    protected void SetVisualWheelsPositionAndRotation()
    {
        SetWheelPositionAndRotation(wheelBR, wheelBRVisual);
        SetWheelPositionAndRotation(wheelBL, wheelBLVisual);
        SetWheelPositionAndRotation(wheelFR, wheelFRVisual);
        SetWheelPositionAndRotation(wheelFL, wheelFLVisual);
    }

    protected void SetWheelsBrakeTorque(float brakingApplied)
    {
        wheelBL.brakeTorque = brakingApplied;
        wheelBR.brakeTorque = brakingApplied;
        wheelFL.brakeTorque = brakingApplied;
        wheelFR.brakeTorque = brakingApplied;
    }
    
    private void SetWheelPositionAndRotation(WheelCollider wheelCollider, Transform wheelVisual)
    {
        wheelCollider.GetWorldPose(out var wheelPosition, out var wheelRotation);
        wheelVisual.transform.position = wheelPosition;
        wheelVisual.transform.rotation = wheelRotation;
    }
}