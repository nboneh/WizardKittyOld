using UnityEngine;
using System.Collections;

public class WizardKitty : MonoBehaviour {

    public Camera followCamera;
    float moveSpeed = 5;
    float distToGround;


    int forwardMovement = 0;
    int sideMovement = 0;


    float minimumCameraElevation = -50F;
    float maximumCameraElevation = 50F;

    float cameraAzimuth = 0;
    float cameraElevation = 15;
    Vector3[] prevPos;

    // Use this for initialization
    void Start()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y; 
    }

    bool IsGrounded() {
      return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
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
                mainYaw += Mathf.Abs(mainYaw - cameraYaw) *.05f* t * 150;
                if (mainYaw >= cameraYaw)
                {
                    mainYaw = cameraYaw;
                }
            }
            else if (mainYaw > cameraYaw)
            {
                mainYaw -= Mathf.Abs(mainYaw - cameraYaw) *.05f * t * 150;
                if (mainYaw <= cameraYaw)
                {
                    mainYaw = cameraYaw;
                }
            }
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, mainYaw, transform.rotation.eulerAngles.z));
        }

            float speed = moveSpeed;
            if (forwardMovement != 0 && sideMovement != 0)
                speed *= Mathf.Sqrt(2) / 2;

          
            Vector3 forward = followCamera.transform.forward;
            Vector3 right = followCamera.transform.right;
        forward.y = 0;
        right.y = 0;
 
        transform.Translate(forwardMovement * forward * speed * t - sideMovement * right * speed * t, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, 7, rb.velocity.z);
        }
    }




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
