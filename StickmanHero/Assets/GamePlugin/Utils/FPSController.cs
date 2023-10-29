using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float xShow = 0;
    public float yShow = 0.5f;
    private float deltaTime;

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        var msec = deltaTime * 1000.0f;
        var fps = 1.0f / deltaTime;
        var text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        int w = Screen.width, h = Screen.height;
        var style = new GUIStyle();
        var rect = new Rect(xShow * w, yShow * h, w, 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 5 / 110;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = new Color(1, 239.0f / 255.0f, 0, 1);
        GUI.Label(rect, text, style);
    }
}