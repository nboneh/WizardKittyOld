using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {


    public WizardKitty kitty;
    public ThirdPersonCamera.FreeForm cameraRotate;
    public Camera mainCamera;
    public StaffScript staff;
    int numberOfTerrains;
    int currentTerrain = 0;
    Vector3 initialPosition;
    public Texture cloubyLogo;

    float GUITimeCounter = 0;
    float cloubyLogoFadeInAt = 4;
    float cloubyLogoFadeOutAt = 10;


    int MoveKittyDistance = 400;
    int MoveKittyEvery = 6;
    float moveKittyCounter = 0;
    float camSpeedX = 15;
    float camSpeedY = 0;

    float alpha = 0.0f;

    public Material[] skyboxes;
    public string[] layerNames;
    Vector3[] kittyOffSets = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
    StaffScript.Spell[] spells = new StaffScript.Spell[] { StaffScript.Spell.Ice, StaffScript.Spell.Fire , StaffScript.Spell.Nature};
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        kitty.setForwardMovement(WizardKitty.Direction.Backward);
        kitty.setSideMovement(WizardKitty.Direction.Left);
        initialPosition = kitty.transform.position;
        numberOfTerrains = layerNames.Length;
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
        RotateCamera(t);

    }

    void RotateCamera(float t)
    {
        cameraRotate.addRotationToX(t * camSpeedX);
        cameraRotate.addRotationToY(t * camSpeedY);
    }

    void ChangeTerrain()
    {
        currentTerrain++;
        if (currentTerrain >= numberOfTerrains)
            return;
        Vector3 kittyOffSet = kittyOffSets[currentTerrain];
        kitty.transform.position = new Vector3(initialPosition.x + kittyOffSet.x, initialPosition.y + kittyOffSet.y, initialPosition.z + kittyOffSet.z - currentTerrain*MoveKittyDistance);
        Skybox skybox = mainCamera.GetComponent<Skybox>();
        skybox.material = skyboxes[currentTerrain];
        //Switch between which layer to render
        HideLayer(layerNames[currentTerrain - 1]);
        ShowLayer(layerNames[currentTerrain]);
        staff.SetSpell(spells[currentTerrain]);
    }

    void OnGUI()
    {
        float t = Time.deltaTime;
        if (GUITimeCounter >= cloubyLogoFadeInAt && GUITimeCounter <= cloubyLogoFadeOutAt)
        {
            alpha += t /2;
        } else if (GUITimeCounter >= cloubyLogoFadeOutAt)
        {
            alpha -= t / 2;
        }

        if (alpha > 1.0f)
        {
            alpha = 1.0f;
        }
        SetAlpha();
        DrawCloubyLogo();
        GUITimeCounter += t;
    }

    void SetAlpha()
    {
        Color c = GUI.color;
        c.a = alpha;
        GUI.color = c;
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
