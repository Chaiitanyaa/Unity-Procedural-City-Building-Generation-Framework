using UnityEngine;

public class DisplayControls : MonoBehaviour
{
    [TextArea(5, 12)]
    public string controlsText =
        "Scene 3 Controls:\n" +
        "\n" +
        "C  = Generate new city\n" +
        "X  = Clear city\n" +
        "\n" +
        "W A S D = Move\n" +
        "Q / E   = Down / Up\n" +
        "Arrow Keys = Look around\n" +
        "Shift = Move faster\n";

    public int fontSize = 16;
    public float padding = 10f;

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = fontSize;
        style.normal.textColor = Color.white;

        // Draw semi-transparent background
        Color oldColor = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, 0.4f);
        GUI.Box(new Rect(padding, Screen.height - 250, 320, 220), "");
        GUI.color = oldColor;

        GUI.Label(new Rect(padding, Screen.height - 250, 320, 220), controlsText, style);
    }
}