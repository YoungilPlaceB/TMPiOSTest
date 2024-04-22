using TMPro;
using UnityEngine;

public class MyTMP_Input : MonoBehaviour
{
    TMP_InputField tmp_inputField;
    Helper helper;

    private void Awake()
    {
        tmp_inputField = GetComponent<TMP_InputField>();
        helper = gameObject.AddComponent<Helper>();
        helper.Init(tmp_inputField);
        helper.SetEnable(true);
    }
}
