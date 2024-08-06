using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHorseCart : MonoBehaviour
{
    [Header("Center of Mass")]
    private Rigidbody rb;
    private Vector3 CentOfMass = new Vector3(0f, -0.5f, 0f);
    [Header("Path")]
    [SerializeField] private Transform path;
    [SerializeField] private Transform[] pathTransforms;
    [SerializeField] private List<Transform> pathList;
    [Header("Wheel Collider")]
    [SerializeField] private WheelCollider FrontL;
    [SerializeField] private WheelCollider FrontR;
    [SerializeField] private WheelCollider BackL;
    [SerializeField] private WheelCollider BackR;

    public float curSpeed = 0;              // 현재 속도
    private float maxSpeed = 100f;          // 최대 속도
    private int curNode = 0;                // 현재 노드
    private float maxTorque = 500f;                     // 최대 토크
    private float maxSteerAngle = 35f;                   // 최대 조향각
    private float maxBrake = 150000f;                    // 최대 브레이크

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CentOfMass;
        path = GameObject.Find("PathTransform").transform;
        pathTransforms = path.GetComponentsInChildren<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path)
                pathList.Add(pathTransforms[i]);
        }
    }

    void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWayPointDistance();
    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(pathList[curNode].position);   // 현재 노드의 위치를 상대좌표로 변환합니다.
        float newSteer = relativeVector.x / relativeVector.magnitude * maxSteerAngle;           // 조향각을 계산합니다.
        FrontL.steerAngle = newSteer;                                               // 좌측 앞바퀴의 조향각을 설정합니다.
        FrontR.steerAngle = newSteer;                                               // 우측 앞바퀴의 조향각을 설정합니다.
    }

    void Drive()
    {
        curSpeed = 2 * Mathf.PI * FrontL.radius * FrontL.rpm * 60 / 1000;           // 현재 속도를 계산합니다.

        if (curSpeed < maxSpeed)
        {
            BackL.motorTorque = maxTorque;
            BackR.motorTorque = maxTorque;
        }
        else
        {
            BackL.motorTorque = 0;
            BackR.motorTorque = 0;
        }
    }

    void CheckWayPointDistance()
    {        
        if (Vector3.Distance(transform.position, pathList[curNode].position) <= 20f)       // 현재 노드와의 거리가 20f 이하일 경우
        {
            if (curNode == pathList.Count - 1)                                              // 현재 노드가 마지막 노드일 경우
                curNode = 0;                                                                // 첫 번째 노드로 이동합니다.
            else
                curNode++;                                                                  // 다음 노드로 이동합니다.
        }

    }
}
