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
        SetMotorTorque(parMotor);
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
            RaycastHit hit;
            var ray = new Ray(sensor.transform.position, sensor.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * sensorsDistance, Color.red, 0.1f);

            if (!Physics.Raycast(ray, out hit, sensorsDistance)) continue;

            switch (sensor.sensorPosition)
            {
                case SensorPosition.FrontLeft:
                    Debug.Log("Front left");
                    wheelRotationMultiplier = 1;
                    avoiding = true;
                    break;
                case SensorPosition.FrontLeftDiagonal:
                    Debug.Log("Front left diag");
                    if (avoiding) continue;
                    avoiding = true;
                    wheelRotationMultiplier = 0.6f;
                    break;
                case SensorPosition.FrontRight:
                    Debug.Log("Front right");
                    wheelRotationMultiplier = -1;
                    avoiding = true;
                    break;
                case SensorPosition.FrontRightDiagonal:
                    Debug.Log("Front right diag");
                    if (avoiding) continue;
                    avoiding = true;
                    wheelRotationMultiplier = -0.6f;
                    break;

                case SensorPosition.Front:
                    Debug.DrawRay(hit.transform.position, hit.normal * 10f, Color.green);
                    var rotation = Quaternion.FromToRotation(transform.forward, hit.normal);
                    var eulerRotation = rotation.eulerAngles;
                    wheelRotationMultiplier = eulerRotation.y > 180 ? 1 : -1;
                    avoiding = true;
                    break;
            }
        }

        if (avoiding)
        {
            SetSteeringAngle(maxWheelRotation * wheelRotationMultiplier);
        }
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
                SetWheelsBrakeTorque(brakeForce);
            }
            else
            {
                ToggleBrakeLights(0);
                SetWheelsBrakeTorque(0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BrakeZone>(out _))
        {
            brakeZone = null;
            ToggleBrakeLights(0);
            SetWheelsBrakeTorque(0);
        }
    }
}