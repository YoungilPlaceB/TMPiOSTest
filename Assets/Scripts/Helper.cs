using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Helper : MonoBehaviour, IPointerClickHandler
{
    TMP_InputField _tmpInputField;

    TouchScreenKeyboard _keyboard;
    bool initialized = false;
    public bool Enable { get; private set; }

    [SerializeField] PasswordInputFieldSimulator _passwordInputFieldSimulatorPrefab;
    PasswordInputFieldSimulator _passwordInputFieldSimulator;

    string _orgText;

    public void Init(TMP_InputField passwordInputUI)
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            return;

        _tmpInputField = passwordInputUI;
        initialized = true;
    }

    public void SetEnable(bool enable)
    {
        if (!initialized)
            return;

        Enable = enable;

        _tmpInputField.shouldHideMobileInput = Enable;
        _tmpInputField.shouldHideSoftKeyboard = Enable;
    }

    void CreatePasswordSimulator()
    {
        if(_passwordInputFieldSimulatorPrefab != null)
        {
            _passwordInputFieldSimulator = Instantiate(_passwordInputFieldSimulatorPrefab);
            _passwordInputFieldSimulator.Init(_keyboard);

            _passwordInputFieldSimulator.onCancelButtonClicked.AddListener(() =>
            {
                _tmpInputField.text = _orgText;
                CloseKeyboard();
            });

            _passwordInputFieldSimulator.onOKButtonClicked.AddListener(() =>
            {
                _keyboard.text = _passwordInputFieldSimulator.InputField.text;
                CloseKeyboard();
            });
        }
    }

    void DestroyPasswordSimulator()
    {
        if(_passwordInputFieldSimulator != null)
        {
            _passwordInputFieldSimulator.onOKButtonClicked.RemoveAllListeners();
            _passwordInputFieldSimulator.onCancelButtonClicked.RemoveAllListeners();

            Destroy(_passwordInputFieldSimulator.gameObject);
            _passwordInputFieldSimulator = null;
        }
    }

    void CloseKeyboard()
    {
        if (_keyboard != null)
        {
            _keyboard.active = false;
        }

        _keyboard = null;
        DestroyPasswordSimulator();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!initialized || !Enable)
            return;

        var temp_keyboard = TouchScreenKeyboard.Open(_tmpInputField.text, TouchScreenKeyboardType.Default, false, false, false, false, "", _tmpInputField.characterLimit);
        TouchScreenKeyboard.hideInput = true;
        temp_keyboard.active = false;

        _keyboard = TouchScreenKeyboard.Open(_tmpInputField.text, TouchScreenKeyboardType.Default, false, false, false, false, "", _tmpInputField.characterLimit);
        TouchScreenKeyboard.hideInput = true;

        _orgText = _tmpInputField.text;
        CreatePasswordSimulator();
    }


    void LateUpdate()
    {
        if (!initialized || !Enable || _keyboard == null)
        {
            CloseKeyboard();
            return;
        }

        if (_keyboard.status != TouchScreenKeyboard.Status.Visible)
        {
            CloseKeyboard();
            return;
        }

        if(_passwordInputFieldSimulator == null)
        {
            CloseKeyboard();
            return;
        }

        _tmpInputField.text = _keyboard.text;
    }
}
