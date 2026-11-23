using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class ForceReceiver : MonoBehaviour
{
    [field: SerializeField] private CharacterController controller;
    [field: SerializeField] private NavMeshAgent agent;
    [field: SerializeField] private float drag = 0.3f;
    private float _verticalVelocity;
    private Vector3 _impactForce;
    private Vector3 _dampingVelocity;

    public Vector3 Movement => _impactForce + Vector3.up * _verticalVelocity;
    private void Update()
    {
        if (_verticalVelocity < 0f && controller.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        _impactForce = Vector3.SmoothDamp(_impactForce, Vector3.zero,ref _dampingVelocity, drag);

        if (_impactForce.sqrMagnitude <= 0.2f*0.2f && agent)
        {
            _impactForce = Vector3.zero;
            agent.enabled = true;
        }
    }
    
    public void AddForce(Vector3 force)
    {
        _impactForce += force;
        if (agent)
        {
            agent.enabled = false;
        }
    }
}
