using UMI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    Canvas _canvas;

    [SerializeField]
    private RectTransform _inputField;

    private void Update()
    {
        float height = Utility.GetKeyboardHeight();

        float h = height / _canvas.scaleFactor;
        _inputField.anchoredPosition = new Vector2(0, h);
    }
}
