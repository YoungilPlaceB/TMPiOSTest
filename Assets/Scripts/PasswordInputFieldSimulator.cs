using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PasswordInputFieldSimulator : MonoBehaviour
{
    [SerializeField] Canvas _canvas;

    [SerializeField] TMP_InputField _inputField;
    [SerializeField] RectTransform _background;
    [SerializeField] Button _clearButton;

    [SerializeField] TMP_InputField _inputField_dark;
    [SerializeField] RectTransform _background_dark;
    [SerializeField] Button _clearButton_dark;

    public UnityEvent onCloseAreaClicked = new UnityEvent();
    public UnityEvent onOKButtonClicked = new UnityEvent();
    public UnityEvent onCancelButtonClicked = new UnityEvent();

    public TMP_InputField InputField
    {
        get
        {
            if(isDark)
                return _inputField_dark;
            else
                return _inputField;
        }
    }       

    public RectTransform Background
    {
        get
        {
            if(isDark)
                return _background_dark;
            else
                return _background;
        }
    }

    public Button ClearButton
    {         
        get
        {
            if(isDark)
                return _clearButton_dark;
            else
                return _clearButton;
        }
    }

    TouchScreenKeyboard _keyboard;
    string storedText = "";
    bool isDark = false;

    public void Init(TouchScreenKeyboard keyboard, bool isDark = false)
    {
        _keyboard = keyboard;
        this.isDark = isDark;
        if (isDark)
        {
            _background.gameObject.SetActive(false);
            _background_dark.gameObject.SetActive(true);
        }
        else
        {
            _background.gameObject.SetActive(true);
            _background_dark.gameObject.SetActive(false);
        }
    }

    void MaskText()
    {
        string maskedText = "";
        for (int i = 0; i < _keyboard.text.Length-1; i++)
        {
            maskedText += "⦁";
        }

        InputField.text = maskedText;

        if(_keyboard.text.Length > 0)
            InputField.text += _keyboard.text[_keyboard.text.Length - 1];
    }    

    private void LateUpdate()
    {
        if(_keyboard == null)
            return;

        float height = Utility.GetKeyboardHeight();

        float h = height / _canvas.scaleFactor;
        Background.anchoredPosition = new Vector2(0, h);

        if(storedText != _keyboard.text)
        {
            MaskText();
            storedText = _keyboard.text;
        }

        ClearButton.gameObject.SetActive(_keyboard.text.Length > 0);
    }

    public void OnCloseAreaClicked()
    {
        if(_keyboard != null)
            _keyboard.active = false;

        Destroy(gameObject);
        onCloseAreaClicked.Invoke();
    }

    public void OnCancelButtonClicked()
    {
        onCancelButtonClicked.Invoke();        
    }

    public void OnOKButtonClicked()
    {
        onOKButtonClicked.Invoke();
    }

    public void OnClearButtonClicked()
    {
        _keyboard.text = "";
    }
}
