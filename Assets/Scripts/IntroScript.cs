using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {


    public WizardKitty kitty;
    public Burnable mainTree;
    bool mainTreeBurnt = false;
    public ThirdPersonCamera.FreeForm cameraRotate;
    public Camera mainCamera;
    public StaffScript staff;
    int numberOfTerrains;
    int currentTerrain = 0;
    Vector3 initialPosition;
    public Texture cloubyLogo;
    public AudioSource introSong;

    bool drawPressAnyButton = false;
    bool increaseAlphaForAnyButton = true;

    float GUITimeCounter = 0;
    float cloubyLogoFadeInAt = 4;
    float cloubyLogoFadeOutAt = 10;


    int MoveKittyDistance = 400;
    float MoveKittyEvery = 7f;
    float moveKittyCounter = 0;

    float camSpeedX = 0;
    float camSpeedY = 0;
    float zoomOutSpeed = 0;
    float camTime = 0;

    float alpha = 0.0f;

    float camDuration;

    public Material[] skyboxes;
    public string[] layerNames;
    Vector3[] kittyOffSets = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0),  new Vector3(-13, 0, -13) };
    StaffScript.Spell[] spells = new StaffScript.Spell[] { StaffScript.Spell.Nature, StaffScript.Spell.Ice , StaffScript.Spell.Lightning, StaffScript.Spell.Fire };

    float[] camDurations = new float [] { 7f, 6.7f, 7f, 7f};
    float[] camSpeedsX = new float[] { 14, 17, -15, -25 };
    float[] camSpeedsY = new float[] { .2f, -3, 2, -2 };
    float[] zoomOutSpeeds = new float[] { 1.5f, -.5f, -.7f, 2f};
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        kitty.setForwardMovement(WizardKitty.Direction.Backward);
        kitty.setSideMovement(WizardKitty.Direction.Left);
        initialPosition = kitty.transform.position;
        numberOfTerrains = layerNames.Length;
        introSong.Play();

        camSpeedX = camSpeedsX[0];
        camSpeedY = camSpeedsY[0];
        camDuration = camDurations[0];
        zoomOutSpeed = zoomOutSpeeds[0];
    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.deltaTime;
        moveKittyCounter += t;
        if (moveKittyCounter >= MoveKittyEvery)
        {
            ChangeTerrain();
            moveKittyCounter = 0;
        }
        UpdateCamera(t);
        if (!mainTreeBurnt && mainTree.Burnt())
        {
            camTime = 0;
            camSpeedX = 18f;
            camSpeedY = 2f;
            zoomOutSpeed = -2f;
            camDuration = 6.5f;
            mainTreeBurnt = true;
            kitty.setForwardMovement(WizardKitty.Direction.Backward);
            kitty.setSideMovement(WizardKitty.Direction.Left);

        }
    }

    void UpdateCamera(float t)
    {
        if (camTime >= camDuration)
        {
            if (mainTreeBurnt)
            {
                kitty.setForwardMovement(WizardKitty.Direction.NoDirection);
                kitty.setSideMovement(WizardKitty.Direction.NoDirection);
                drawPressAnyButton = true;
            }
            return;
        }
        cameraRotate.AddRotationToX(t * camSpeedX);
        cameraRotate.AddRotationToY(t * camSpeedY);
        cameraRotate.AddDistance(t * zoomOutSpeed);
        camTime += t;
    }

    void ChangeTerrain()
    {
        currentTerrain++;
        if (currentTerrain > numberOfTerrains)
        {
            return;
        }
        if(currentTerrain == numberOfTerrains)
        {
            staff.ShootFireBall(kitty.transform.forward);
            kitty.setSideMovement(WizardKitty.Direction.NoDirection);
            kitty.setForwardMovement(WizardKitty.Direction.NoDirection);
            return;
        }

        Vector3 kittyOffSet = kittyOffSets[currentTerrain];
        kitty.transform.position = new Vector3(initialPosition.x + kittyOffSet.x, initialPosition.y + kittyOffSet.y, initialPosition.z + kittyOffSet.z - currentTerrain*MoveKittyDistance);
        Skybox skybox = mainCamera.GetComponent<Skybox>();
        skybox.material = skyboxes[currentTerrain];
        //Switch between which layer to render
        HideLayer(layerNames[currentTerrain - 1]);
        ShowLayer(layerNames[currentTerrain]);
        staff.SetSpell(spells[currentTerrain]);

        camTime = 0;
        camSpeedX = camSpeedsX[currentTerrain];
        camSpeedY = camSpeedsY[currentTerrain];
        zoomOutSpeed = zoomOutSpeeds[currentTerrain];
        camDuration = camDurations[currentTerrain];

    }

    void OnGUI()
    {
        float t = Time.deltaTime;
        if (drawPressAnyButton)
        {
            if (increaseAlphaForAnyButton)
            {
                alpha += t /2;
                if(alpha >= 1.0f)
                {
                    increaseAlphaForAnyButton = false;
                }
            }
            else
            {
                alpha -= t /2;
                if (alpha <= 0.0f)
                {
                    increaseAlphaForAnyButton = true;
                }
            }
        }
        else
        {
            if (GUITimeCounter >= cloubyLogoFadeInAt && GUITimeCounter <= cloubyLogoFadeOutAt)
            {
                alpha += t / 2;
            }
            else if (GUITimeCounter >= cloubyLogoFadeOutAt)
            {
                alpha -= t / 2;
            }
        }

        if (alpha > 1.0f)
        {
            alpha = 1.0f;
        } else if(alpha < 0.0f)
        {
            alpha = 0.0f;
        }
        SetAlpha();
        if (drawPressAnyButton)
            DrawPressAnyButton();
        else
            DrawCloubyLogo();
        GUITimeCounter += t;
    }

    void SetAlpha()
    {
        Color c = GUI.color;
        c.a = alpha;
        GUI.color = c;
    }

    void DrawPressAnyButton()
    {
        int fontSize = GetFontSize(25);

        GUIStyle textStyle = GUI.skin.GetStyle("Label");
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.normal.textColor = Color.white;
        textStyle.fontSize = fontSize;
        textStyle.fontStyle = FontStyle.Bold;


        float width = fontSize * 36;
        float height = fontSize * 2;
        float screenWidthHalf = Screen.width / 2.0f;
        float yPosition = Screen.height * (5.0f/6.0f);


        DrawOutline(new Rect(screenWidthHalf - width / 2.0f, yPosition - height / 2.0f, width, height), "Press Any Button", textStyle);
    }

    void DrawCloubyLogo()
    {
        float screenWidthHalf = Screen.width/2.0f;
        float screenHeightHalf = Screen.height/2.0f;
        float textureWidth = cloubyLogo.width*2;
        float textureHeight = cloubyLogo.height*2;

        GUI.DrawTexture(new Rect(screenWidthHalf - textureWidth/2.0f,screenHeightHalf - textureHeight/2.0f , textureWidth, textureHeight), cloubyLogo, ScaleMode.StretchToFill, true, 10.0F);

    }

    void DrawOutline(Rect position, string text, GUIStyle style)
    {
        var outColor = Color.gray;
        var backupStyle = style;
        var oldColor = style.normal.textColor;
        style.normal.textColor = outColor;
        position.x--;
        GUI.Label(position, text, style);
        position.x += 2;
        GUI.Label(position, text, style);
        position.x--;
        position.y--;
        GUI.Label(position, text, style);
        position.y += 2;
        GUI.Label(position, text, style);
        position.y--;
        style.normal.textColor = oldColor;
        GUI.Label(position, text, style);
        style = backupStyle;
    }

    int GetFontSize(float size)
    {
        float BaseFontScaler = Mathf.Min(Screen.width, Screen.height);
        int fontSize = (int)(2.8f * BaseFontScaler * size / 1920.0f); //scale size font;
        if (fontSize < 14)
            fontSize = 14;
        return fontSize;
    }

    private void ShowLayer(string layerName)
    {
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
    }

    // Turn off the bit using an AND operation with the complement of the shifted int:
    private void HideLayer(string layerName)
    {
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
    }
}
