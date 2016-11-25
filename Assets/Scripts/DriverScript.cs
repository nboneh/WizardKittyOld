using UnityEngine;
using System.Collections;

public class DriverScript : MonoBehaviour
{
    enum GameState { Play, Pause };
    GameState currentState;
    GameState prevState;
    public Font font;
    AudioSource source;

    public AudioClip pauseSound;
    public AudioClip unPauseSound;

    float alpha = 0.0f;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        /*    Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;*/

        source = GetComponent<AudioSource>();
        font = GetComponent<Font>();

    }
    void SetState(GameState state)
    {
        if (state == GameState.Play)
        {
            //gameSoundTrack.Play();
            //mainMenuSoundTrack.Stop();
            Cursor.lockState = CursorLockMode.Locked;
            ResumeGame();
        }
        else
        {
            // gameSoundTrack.Pause();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseGame();
        }

        prevState = currentState;
        currentState = state;
        alpha = 0;
    }
    void PauseGame()
    {
        Time.timeScale = 0.0F;
    }

    void ResumeGame()
    {
        Time.timeScale = 1.0F;
    }

    void SetAlpha()
    {
        Color c = GUI.color;
        c.a = alpha;
        GUI.color = c;
    }

    void SetPrevAlpha()
    {
        Color c = GUI.color;
        c.a = 1 - alpha;
        GUI.color = c;
    }

    void OnGUI()
    {
        SetPrevAlpha();
        DrawState(prevState);
        SetAlpha();
        DrawState(currentState);
    }


    void DrawState(GameState state)
    {
        switch (state)
        {
            case GameState.Play:
                GUI.color = Color.white;
                break;
            case GameState.Pause:
                DrawBoxMenu(state);
                GUI.color = Color.white;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.unscaledDeltaTime;
        if (t > .3f)
            return;
        if (alpha < 1.0f)
        {
            alpha += t * 2;
            if (alpha > 1.0f)
            {
                alpha = 1.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentState == GameState.Play)
        {
            SetState(GameState.Pause);
            source.PlayOneShot(pauseSound);
        }
    }

    void DrawBoxMenu(GameState state)
    {
        float boxWidth = Screen.width / 2f;
        float boxHeight = Screen.height / 2f;

        float startX = Screen.width / 2 - boxWidth / 2;
        float startY = Screen.height / 2 - boxHeight / 2;

        GUI.Box(new Rect(startX, startY, boxWidth, boxHeight), GUIContent.none);

        GUIStyle textStyle = GUI.skin.GetStyle("Label");
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.normal.textColor = Color.white;
        int fontSize = GetFontSize(50);
        textStyle.fontSize = fontSize;
        textStyle.font = font;

        float width = fontSize * 12;
        float height = fontSize * 2;

        if (state == GameState.Pause)
        {
            DrawOutline(new Rect(Screen.width / 2 - width / 2, startY + height / 16, width, height), "Pause", textStyle);
        }


        fontSize = GetFontSize(30);
        GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
        buttonStyle.fontSize = fontSize;
        buttonStyle.font = font;
        float buttonWidth = fontSize * 4.7f;
        float buttonHeight = fontSize * 1.5f;

        if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, Screen.height - buttonHeight * 7, buttonWidth, buttonHeight), "Resume", buttonStyle) && alpha >= 1.0f)
        {
            SetState(GameState.Play);
            source.PlayOneShot(unPauseSound);
        }
        buttonWidth = fontSize * 4f;
        if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, Screen.height - buttonHeight * 5.5f, buttonWidth, buttonHeight), "Exit", buttonStyle) && alpha >= 1.0f)
        {
            Application.Quit();
        }

    }

    int GetFontSize(float size)
    {
        float BaseFontScaler = Mathf.Min(Screen.width, Screen.height);
        int fontSize = (int)(2.8f * BaseFontScaler * size / 1920.0f); //scale size font;
        if (fontSize < 14)
            fontSize = 14;
        return fontSize;
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
}