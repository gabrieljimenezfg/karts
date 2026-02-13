using System;
using UnityEngine;

public class BaseCar : MonoBehaviour
{
    [SerializeField] protected WheelCollider wheelFR, wheelFL, wheelBR, wheelBL;
    protected Transform wheelFRVisual, wheelFLVisual, wheelBRVisual, wheelBLVisual;
    [SerializeField] protected float parMotor;
    [SerializeField] protected float maxWheelRotation;
    [SerializeField] protected float brakeForce;
    protected MeshRenderer meshRenderer;

    protected virtual void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetWheelVisuals(wheelFR, ref wheelFRVisual);
        SetWheelVisuals(wheelFL, ref wheelFLVisual);
        SetWheelVisuals(wheelBR, ref wheelBRVisual);
        SetWheelVisuals(wheelBL, ref wheelBLVisual);
    }

    private void SetWheelVisuals(WheelCollider wheelCollider, ref Transform wheelVisual)
    {
        wheelVisual = wheelCollider.transform.GetChild(0);
    }
    
    protected void SetWheelPositionAndRotation(WheelCollider wheelCollider, Transform wheelVisual)
    {
        wheelCollider.GetWorldPose(out var wheelPosition, out var wheelRotation);
        wheelVisual.transform.position = wheelPosition;
        wheelVisual.transform.rotation = wheelRotation;
    }
}