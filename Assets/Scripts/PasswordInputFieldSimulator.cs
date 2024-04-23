using TMPro;
using UnityEngine;

public class PasswordInputFieldSimulator : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] RectTransform _background;

    TouchScreenKeyboard _keyboard;
    string storedText = "";

    public void Init(TouchScreenKeyboard keyboard)
    {
        _keyboard = keyboard;
    }

    void MaskText()
    {
        string maskedText = "";
        for (int i = 0; i < _keyboard.text.Length-1; i++)
        {
            maskedText += "*";
        }

        _inputField.text = maskedText;

        if(_keyboard.text.Length > 0)
            _inputField.text += _keyboard.text[_keyboard.text.Length - 1];
    }    

    private void LateUpdate()
    {
        if(_keyboard == null)
            return;

        float height = Utility.GetKeyboardHeight();

        float h = height / _canvas.scaleFactor;
        _background.anchoredPosition = new Vector2(0, h);

        if(storedText != _keyboard.text)
        {
            MaskText();
            storedText = _keyboard.text;
        }
    }

    public void Close()
    {
        if(_keyboard != null)
            _keyboard.active = false;

        Destroy(gameObject);
    }
}
