using UnityEngine;
using System.Collections;

public class WizardKitty : MonoBehaviour {

    public Camera followCamera;
    float moveSpeed = 5;


    int forwardMovement = 0;
    int sideMovement = 0;

    float momentumAngle = 0;
    float maxMomentumAngle = 15;

    float minimumCameraElevation = -50F;
    float maximumCameraElevation = 50F;

    float cameraAzimuth = 0;
    float cameraElevation = 15;


    Collider currentFloor = null;



    // Use this for initialization
    void Start()
    {
  
    }

    

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;
        Movement(t);
        UpdateCamera(t);
    }







    void Movement(float t)
    {
        if (Input.GetKey("w") && Input.GetKey("s"))
        {
            forwardMovement = 0;
        }
        else if (Input.GetKey("w") )
        {
            forwardMovement = 1;
        }

        else if (Input.GetKey("s"))
        {
            forwardMovement = -1;
        }
        else
        {
            forwardMovement = 0;
        }

        if (Input.GetKey("a")&& Input.GetKey("d"))
        {
            sideMovement = 0;
        }
        else if (Input.GetKey("a") )
        {
            sideMovement = 1;
        }

        else if (Input.GetKey("d") )
        {
            sideMovement = -1;
        }
         else
        {
            sideMovement = 0;
        }



        if (forwardMovement != 0)
        {
            float speed = moveSpeed;
            if (sideMovement != 0)
                speed *= Mathf.Sqrt(2) / 2;

            Vector3 forward = followCamera.transform.forward;
            forward.y = 0;
            transform.position += forwardMovement * forward * speed * t;
        }

 
        if (sideMovement != 0)
        {
            float speed = moveSpeed;
            if (forwardMovement != 0)
                speed *= Mathf.Sqrt(2) / 2;

            Vector3 right = followCamera.transform.right;
            right.y = 0;
            transform.position -= sideMovement * right * speed * t;
        }

        if (forwardMovement != 0 || sideMovement != 0)
        {
 
            float cameraYaw = followCamera.transform.eulerAngles.y;
            float mainYaw = transform.rotation.eulerAngles.y;

            if (forwardMovement == 1 && sideMovement == 0)
            {
                cameraYaw += 0;
            }
            else if (forwardMovement == 1 && sideMovement == -1)
            {
                cameraYaw += 45;
            }
            else if (forwardMovement == 0 && sideMovement == -1)
            {
                cameraYaw += 90;
            }
            else if (forwardMovement == -1 && sideMovement == -1)
            {
                cameraYaw += 135;
            }
            else if (forwardMovement == -1 && sideMovement == 0)
            {
                cameraYaw += 180;
            }
            else if (forwardMovement == -1 && sideMovement == 1)
            {
                cameraYaw += 225;
            }
            else if (forwardMovement == 0 && sideMovement == 1)
            {
                cameraYaw += 270;
            }
            else if (forwardMovement == 1 && sideMovement == 1)
            {
                cameraYaw += 315;
            }

            cameraYaw = cameraYaw % 360;


            float cameraYawLow = cameraYaw - 360;
            float cameraYawHigh = cameraYaw + 360;

            float low = Mathf.Abs(cameraYawLow - mainYaw);
            float mid = Mathf.Abs(cameraYaw - mainYaw);
            float high = Mathf.Abs(cameraYawHigh - mainYaw);

            if (low < mid && low < high)
                cameraYaw = cameraYawLow;
            else if (high < low && high < mid)
                cameraYaw = cameraYawHigh;


            if (Mathf.Abs(cameraYaw - mainYaw) < .01f)
            {
                // Do Nothing
            }
            else if (mainYaw < cameraYaw)
            {
                mainYaw += moveSpeed * t * 150;
                if (mainYaw >= cameraYaw)
                {
                    mainYaw = cameraYaw;
                }
            }
            else if (mainYaw > cameraYaw)
            {
                mainYaw -= moveSpeed * t * 150;
                if (mainYaw <= cameraYaw)
                {
                    mainYaw = cameraYaw;
                }
            }
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, mainYaw, transform.rotation.eulerAngles.z));
        }
 
    }

   /* void Float(float t)
    {
        if (currentFloor != null)
        {
            float maxHeight = 25;
            Rigidbody rb = GetComponent<Rigidbody>();
            float x = transform.position.x;
            float z = transform.position.z;
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(x, maxHeight, z), Vector3.down);

            floatingCycleAngle += t * 90;
            float lastFloatingY = floatingY;
            floatingY = Mathf.Sin(floatingCycleAngle * (Mathf.PI / 180.0f) * 2) * .07f;
            float floatingRoll = Mathf.Sin(floatingCycleAngle * (Mathf.PI / 180.0f)) * 5;
            float floatingYaw = Mathf.Sin(floatingCycleAngle * (Mathf.PI / 180.0f) * 3) * 2;
            if (floatingCycleAngle > 360)
            {
                floatingCycleAngle = floatingCycleAngle % 360;
            }

            if (currentFloor.Raycast(ray, out hit, 2.0f * maxHeight))
            {
                transform.position = new Vector3(x, hit.point.y + floatingHeight + floatingY, z);
                if (momentumAngle < .01f)
                    transform.rotation = Quaternion.Euler(new Vector3(floatingRoll, transform.rotation.eulerAngles.y, flipAngle + floatingYaw));
                rb.velocity = new Vector3(0, 0, 0);
            }
        }

    }*/

    void UpdateCamera(float t)
    {
        cameraAzimuth += Input.GetAxis("Mouse X") * 3.0f;
        cameraElevation -= Input.GetAxis("Mouse Y");

        if (cameraElevation <= minimumCameraElevation)
            cameraElevation = minimumCameraElevation;
        else if (cameraElevation >= maximumCameraElevation)
            cameraElevation = maximumCameraElevation;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        Vector3 pos = new Vector3(x, y, z);
        followCamera.transform.position = pos;

        Quaternion newRotation = Quaternion.AngleAxis(cameraAzimuth, Vector3.up);
        newRotation *= Quaternion.AngleAxis(cameraElevation, Vector3.right);

        followCamera.transform.position = pos + -4 * (newRotation * Vector3.forward);
        followCamera.transform.LookAt(transform);
    }






}
