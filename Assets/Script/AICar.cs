using UnityEngine;

public class AICar : BaseCar
{
    [SerializeField] private PathCars path;
    [SerializeField] private float minimumDistanceFromTarget;
    private int currentNodeTarget;
    private BrakeZone brakeZone;

    protected override void Awake()
    {
        base.Awake();
        parMotor = -parMotor;
        currentNodeTarget = 0;
        ToggleBrakeLights(0);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out brakeZone))
        {
            if (brakeZone.maxSpeed < rb.linearVelocity.magnitude * 3.6f)
            {
                ToggleBrakeLights(brakeForce);
                wheelBL.brakeTorque = brakeForce;
                wheelBR.brakeTorque = brakeForce;
                wheelFL.brakeTorque = brakeForce;
                wheelFR.brakeTorque = brakeForce;
            }
            else
            {
                ToggleBrakeLights(0);
                wheelBL.brakeTorque = 0;
                wheelBR.brakeTorque = 0;
                wheelFL.brakeTorque = 0;
                wheelFR.brakeTorque = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BrakeZone>(out _))
        {
            brakeZone = null;
            ToggleBrakeLights(0);
            wheelBL.brakeTorque = 0;
            wheelBR.brakeTorque = 0;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
        }
    }
}