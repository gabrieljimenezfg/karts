using System;
using UnityEngine;

public class AICar : BaseCar
{
    [SerializeField] private PathCars path;
    [SerializeField] private float minimumDistanceFromTarget;
    private Sensor[] sensors;
    [SerializeField] private float sensorsDistance;
    private int currentNodeTarget;
    private BrakeZone brakeZone;

    protected override void Awake()
    {
        base.Awake();
        sensors = GetComponentsInChildren<Sensor>();
        parMotor = -parMotor;
        currentNodeTarget = 0;
        ToggleBrakeLights(0);
    }

    private void FixedUpdate()
    {
        wheelBL.motorTorque = parMotor;
        wheelBR.motorTorque = parMotor;

        var direction = CheckTargetNode();
        SetSteeringAngleFromDirection(direction);
        CheckSensors();
        SetVisualWheelsPositionAndRotation();
    }

    private void CheckSensors()
    {
        float wheelRotationMultiplier = 0;
        bool avoiding = false;

        foreach (var sensor in sensors)
        {
            var ray = new Ray(sensor.transform.position, sensor.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * sensorsDistance, Color.red, 0.1f);

            if (!Physics.Raycast(ray, sensorsDistance)) continue;

            switch (sensor.sensorPosition)
            {
                case SensorPosition.FrontLeft:
                    wheelRotationMultiplier = 1;
                    avoiding = true;
                    break;
                case SensorPosition.FrontLeftDiagonal:
                    if (avoiding) continue;
                    avoiding = true;
                    wheelRotationMultiplier = 0.6f;
                    break;
                case SensorPosition.FrontRight:
                    wheelRotationMultiplier = -1;
                    avoiding = true;
                    break;
                case SensorPosition.FrontRightDiagonal:
                    if (avoiding) continue;
                    avoiding = true;
                    wheelRotationMultiplier = -0.6f;
                    break;

                case SensorPosition.Front:
                    break;
            }
        }

        if (avoiding)
        {
            SetSteeringAngle(maxWheelRotation * wheelRotationMultiplier);
        }
    }

    private void SetSteeringAngle(float angle)
    {
        wheelFR.steerAngle = angle;
        wheelFL.steerAngle = angle;
    }

    private void SetSteeringAngleFromDirection(Vector3 direction)
    {
        var rotation = Quaternion.FromToRotation(transform.forward, direction);

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