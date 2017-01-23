using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {


    public WizardKitty kitty;
    public ThirdPersonCamera.FreeForm mainCamera;
    int numberOfTerrains = 2;
    int currentTerrain = 1;
    Vector3 initialPosition;

    int MoveKittyDistance = 400;
    int MoveKittyEvery = 6;
    float moveKittyCounter = 0;
    float rotationTime = 0;
    float rotateCameraCounter;
    float camSpeedX, camSpeedY;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        kitty.setForwardMovement(WizardKitty.Direction.Backward);
        kitty.setSideMovement(WizardKitty.Direction.Left);
        initialPosition = kitty.transform.position;
        rotateCameraCounter = rotationTime;
        rotateCamera(30, 3, 3f);


    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.deltaTime;
        moveKittyCounter += t;
        if (moveKittyCounter >= MoveKittyEvery)
            ChangeTerrain();
        if (rotateCameraCounter <= rotationTime)
        {
            mainCamera.addRotationToX(t * camSpeedX);
            mainCamera.addRotationToY(t * camSpeedY);
            rotateCameraCounter += t;
        }
	}

    void ChangeTerrain()
    {
        if (currentTerrain >= numberOfTerrains)
            return;
        kitty.transform.position = new Vector3(initialPosition.x + 25, initialPosition.y + 1.1f, initialPosition.z - MoveKittyDistance);
        currentTerrain++;
        rotateCamera(50, -10, 3f);
    }

    void rotateCamera(float camSpeedX, float camSpeedY, float amountOfTime)
    {
        this.camSpeedX = camSpeedX;
        this.camSpeedY = camSpeedY;
        rotationTime = amountOfTime;
        rotateCameraCounter = 0;
    }
}
