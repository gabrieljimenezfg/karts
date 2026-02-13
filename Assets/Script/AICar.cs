using System.Net.Mime;
using UnityEngine;

public class AICar : BaseCar
{
    [SerializeField] private PathCars path;
    [SerializeField] private float minimumDistanceFromTarget;
    private int currentNodeTarget;

    protected override void Awake()
    {
        base.Awake();
        parMotor = -parMotor;
        currentNodeTarget = 0;
    }

    private void FixedUpdate()
    {
        wheelBL.motorTorque = parMotor;
        wheelBR.motorTorque = parMotor;

        var direction = CheckTargetNode();
        SetWheelAngle(direction);
        
        SetWheelPositionAndRotation(wheelBR, wheelBRVisual);
        SetWheelPositionAndRotation(wheelBL, wheelBLVisual);
        SetWheelPositionAndRotation(wheelFR, wheelFRVisual);
        SetWheelPositionAndRotation(wheelFL, wheelFLVisual);
    }

    private void SetWheelAngle(Vector3 direction)
    {
        var rotation = Quaternion.FromToRotation(transform.forward, direction);
        Debug.Log(rotation);
        
        wheelFR.steerAngle = rotation.eulerAngles.y;
        wheelFL.steerAngle = rotation.eulerAngles.y;
    }

    private Vector3 CheckTargetNode()
    {
        Vector3 direction = path.nodes[currentNodeTarget].position - transform.position;
        float distance = direction.magnitude;

        if (distance < minimumDistanceFromTarget)
        {
            currentNodeTarget++;
            if (currentNodeTarget >= path.nodes.Count)
                currentNodeTarget = 0;
        }
        
        return direction;
    }
}