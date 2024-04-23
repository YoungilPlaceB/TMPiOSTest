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

    public void Init(TMP_InputField passwordInputUI)
    {
        //모바일에서만 동작
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
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
        }
    }

    void DestroyPasswordSimulator()
    {
        if(_passwordInputFieldSimulator != null)
        {
            Destroy(_passwordInputFieldSimulator.gameObject);
            _passwordInputFieldSimulator = null;
        }
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

        CreatePasswordSimulator();
    }


    void LateUpdate()
    {
        if (!initialized || !Enable || _keyboard == null)
        {
            DestroyPasswordSimulator();
            return;
        }

        if (_keyboard.status != TouchScreenKeyboard.Status.Visible)
        {
            DestroyPasswordSimulator();
            _keyboard = null;
            return;
        }

        _tmpInputField.text = _keyboard.text;
    }
}
