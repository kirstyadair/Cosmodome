using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public List<GameObject> shipObjects;
    public List<Transform> shipPositions;
    public Camera mainCamera;

    public float dampeningTime = 0.4f;//The time taken for the camera to reajust
    public float screenEdgeBuff = 8f;//The space between the edge of the screen and any objects at the top or bottom of the screen
    public float minSize = 2f; //Minimum orthographic size the cam can be

  
    private float zoomSpeed; //Speed for the smoothing of the orphographic
    private Vector3 moveVelocity; //Speed for the smoothing of the camera movement
    public Vector3 desiredPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private Vector3 FindCenter(List<Transform>targets)
    {
        Vector3 center;
        Vector3 minPoint = targets[0].position;
        Vector3 maxPoint = targets[0].position;

        for(int i = 1; i<targets.Count; i++)
        {
            
            Vector3 pos = targets[i].position;

            if (pos.x < minPoint.x)
                minPoint.x = pos.x;
            if (pos.x > maxPoint.x)
                maxPoint.x = pos.x;
            if (pos.y < minPoint.y)
                minPoint.y = pos.y;
            if (pos.y > maxPoint.y)
                maxPoint.y = pos.y;
            if (pos.z < minPoint.z)
                minPoint.z = pos.z;
            if (pos.z > maxPoint.z)
                maxPoint.z = pos.z;
        }

       
        center = minPoint + .5f * (maxPoint - minPoint);

        center.y = transform.position.y;
        center.z = transform.position.z;


        return center;

    }

    private void Move()
    {
        desiredPos = FindCenter(shipPositions);
        
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref moveVelocity, dampeningTime);
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize(shipPositions);
        mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, requiredSize, ref zoomSpeed, dampeningTime);
    }


    private float FindRequiredSize(List<Transform> targets)
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPos);

        float size = 0f;
        
        for(int i =1; i<targets.Count;i++)
        {
            
            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x));

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.z));
        }

        size += screenEdgeBuff;

        size = Mathf.Max(size, minSize);

        return size;

    }

    public void SetStartPositionAndSize()
    {
        FindCenter(shipPositions);

        mainCamera.transform.position = desiredPos;

        

    }
    // Update is called once per frame
    void LateUpdate()
    {

        Move();
        //zoom is the problem
        //Zoom();

    }
}
