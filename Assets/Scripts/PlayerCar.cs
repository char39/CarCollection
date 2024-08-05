using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft_Col;
    public WheelCollider frontRight_Col;
    public WheelCollider rearLeft_Col;
    public WheelCollider rearRight_Col;
    [Header("Wheel Models")]
    public Transform frontLeft_Model;
    public Transform frontRight_Model;
    public Transform rearLeft_Model;
    public Transform rearRight_Model;
    [Header("Mass Balance")]
    public Vector3 centerOfMass_var = new Vector3(0f, -0.5f, 0f);   // 무게중심 설정. 높이를 조절하여 차량의 무게중심을 조절할 수 있다.
    public Rigidbody rb;
    [Header("Front Wheel Max Steer Angle")]
    public float maxSteerAngle = 35f;
    [Header("Max Torque")]
    public float maxTorque = 2500f;
    [Header("Max Brake Force")]
    public float maxBrakeForce = 3500f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass_var;
    }

    void FixedUpdate()
    {
        
    }
}
