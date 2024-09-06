using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 무게중심
// 2. pathTransform 참조
// 3. 타이어의 휠 Collider 배치, 모델 배치

public class AICar : MonoBehaviour
{
    [Header("Center of Mass")]
    private Transform tr;
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
    [Header("Models")]
    [SerializeField] private Transform FrontL_Tr;
    [SerializeField] private Transform FrontR_Tr;
    [SerializeField] private Transform BackL_Tr;
    [SerializeField] private Transform BackR_Tr;

    [Header("Obstacle")]
    private LayerMask trackLayer;
    private float sensorLength = 40f;
    private Vector3 FrontSensorPos = new(0f, 0.2f, 0.5f);    // 전방 센서 위치
    //private Transform StartSensorPos;                      // 센서 시작 위치
    private float FrontSideSensorPos = 0.2f;               // 측면 전방 센서 위치
    private float FrontSideSensorAngle = 15f;              // 측면 센서 회전 각도
    private float targetSteerAngle = 0;                                     // 목표 조향각
    private float avoidMultiplier;

    public float curSpeed = 0;              // 현재 속도
    private float maxSpeed = 100f;          // 최대 속도
    private int curNode = 0;                // 현재 노드
    private float maxTorque = 500f;                     // 최대 토크
    private float maxSteerAngle = 30f;                   // 최대 조향각
    private float maxBrake = 150000f;                    // 최대 브레이크

    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CentOfMass;
        path = GameObject.Find("PathTransform").transform;
        pathTransforms = path.GetComponentsInChildren<Transform>();
        trackLayer = 1 << LayerMask.NameToLayer("Track");

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

    private void ApplySteer()
    {
        CarSensor();
        Vector3 relativeVector = transform.InverseTransformPoint(pathList[curNode].position);   // 현재 노드의 위치를 상대좌표로 변환합니다.
        float newSteer = relativeVector.x / relativeVector.magnitude * maxSteerAngle;           // 조향각을 계산합니다.
        newSteer += avoidMultiplier * maxSteerAngle;
        targetSteerAngle = newSteer;
        LerpToAngle();
    }

    private void Drive()
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

    private void CarSensor()
    {
        avoidMultiplier = 0f;
        CheckFrontSensor(tr.position, tr.forward, Color.green);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle, tr.up), -6f, 0.2f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle, tr.up), 6f, 0.2f, ref avoidMultiplier, Color.green, Color.yellow);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle * 0.75f, tr.up), -6f, 0.2f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle * 0.75f, tr.up), 6f, 0.2f, ref avoidMultiplier, Color.green, Color.yellow);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle * 0.5f, tr.up), -6f, 0.5f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle * 0.5f, tr.up), 6f, 0.5f, ref avoidMultiplier, Color.green, Color.yellow);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle * 0.4f, tr.up), -6f, 0.4f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle * 0.4f, tr.up), 6f, 0.4f, ref avoidMultiplier, Color.green, Color.yellow);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle * 0.3f, tr.up), -6f, 0.3f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle * 0.3f, tr.up), 6f, 0.3f, ref avoidMultiplier, Color.green, Color.yellow);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle * 0.2f, tr.up), -6f, 0.2f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle * 0.2f, tr.up), 6f, 0.2f, ref avoidMultiplier, Color.green, Color.yellow);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(-FrontSideSensorAngle * 0.1f, tr.up), -6f, 0.1f, ref avoidMultiplier, Color.green, Color.blue);
        CheckSideSensor(tr.position, tr.forward, Quaternion.AngleAxis(FrontSideSensorAngle * 0.1f, tr.up), 6f, 0.1f, ref avoidMultiplier, Color.green, Color.yellow);
    }

    private void CheckFrontSensor(Vector3 startPos, Vector3 direction, Color color1)
    {
        RaycastHit hit;
        Vector3 sensorStartPos = startPos;
        sensorStartPos += direction * FrontSensorPos.z + tr.up * FrontSensorPos.y;
        bool avoiding = false;

        if (Physics.Raycast(sensorStartPos, direction, out hit, sensorLength, ~trackLayer))
        {
            avoiding = true;
            //avoidMultiplier += 1;
        }
        Debug.DrawLine(sensorStartPos, sensorStartPos + direction * sensorLength, avoiding ? Color.red : color1);
    }

    private void CheckSideSensor(Vector3 startPos, Vector3 direction, Quaternion rotation, float sideOffset, float multiplier, ref float avoidMultiplier, Color color1, Color color2)
    {
        RaycastHit hit;
        Vector3 sensorStartPos = startPos;
        sensorStartPos += direction * FrontSensorPos.z + tr.up * FrontSensorPos.y;
        sensorStartPos += sideOffset * FrontSideSensorPos * tr.right;
        bool avoiding = false;

        if (Physics.Raycast(sensorStartPos, rotation * direction, out hit, sensorLength, ~trackLayer))
        {
            avoiding = true;
            avoidMultiplier += sideOffset > 0 ? -multiplier : multiplier;
        }
        Debug.DrawLine(sensorStartPos, sensorStartPos + rotation * direction * sensorLength, avoiding ? Color.red : color2);
    }

    private void CheckWayPointDistance()
    {
        Vector3 localNodePosition = transform.InverseTransformPoint(pathList[curNode].position);
        bool isNodeBehind = localNodePosition.z < 30f;
        bool isNodeClose = localNodePosition.magnitude < 40f;
        if (isNodeBehind && isNodeClose)
        {
            if (curNode == pathList.Count - 1)                                              // 현재 노드가 마지막 노드일 경우
                curNode = 0;                                                                // 첫 번째 노드로 이동합니다.
            else
                curNode++;                                                                  // 다음 노드로 이동합니다.
        }
    }

    private void LerpToAngle()
    {
        FrontL.steerAngle = Mathf.Lerp(FrontL.steerAngle, targetSteerAngle, Time.deltaTime * 10f);
        FrontR.steerAngle = Mathf.Lerp(FrontR.steerAngle, targetSteerAngle, Time.deltaTime * 10f);
    }
}
