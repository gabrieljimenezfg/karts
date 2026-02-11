using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float damping;

    private void LateUpdate()
    {
        transform.position = player.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, damping);
    }
}