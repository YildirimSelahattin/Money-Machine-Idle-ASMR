using UnityEngine;
using UnityEngine.UI;

public class StartScreenRes1 : MonoBehaviour
{
    public Canvas _Canvas;

    void Start()
    {
        int _width = Display.main.systemWidth;

        Debug.Log("Width: " + _width);

        if (_width > 1450)
        {
            _Canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1540, 1080);
        }
    }
}