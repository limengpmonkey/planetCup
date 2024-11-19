using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using WeChatWASM;
public class UiControl : MonoBehaviour
{
    private Button _button;

    private int _clickCount;


    [SerializeField] private GameObject LevelOne; 
    [SerializeField] private GameObject Login; 
    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {

        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        
        _button = uiDocument.rootVisualElement.Q("playbutton") as Button;
        //
        _button.RegisterCallback<ClickEvent>(PrintClickMessage);
        //
        // var _inputFields = uiDocument.rootVisualElement.Q("input-message");
        // _inputFields.RegisterCallback<ChangeEvent<string>>(InputMessage);
    }

    private void OnDisable()
    {
        Login.SetActive(false);
        LevelOne.SetActive(true);
    }

    private void Start()
    {
#if !UNITY_EDITOR
            WX.InitSDK((int code)=> {
            // 你的主逻辑
            var uiDocument = GetComponent<UIDocument>();
            _button = uiDocument.rootVisualElement.Q("playbutton") as Button;
            _button.RegisterCallback<ClickEvent>(PrintClickMessage);
        });
#endif
        var uiDocument = GetComponent<UIDocument>();
        _button = uiDocument.rootVisualElement.Q("playbutton") as Button;
        _button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        ++_clickCount;
        Debug.Log($"{"button"} was clicked!" +
                  ( " Count: " + _clickCount ));

        this.gameObject.SetActive(false);
        //WX SDK API
        // var button = WX.CreateUserInfoButton(10, 76, 200, 40, "helloha", true);
        // button.OnTap((res) =>
        // {
        //     Debug.Log(res);
        // });
        // button.Show();
    }

    public static void InputMessage(ChangeEvent<string> evt)
    {
        Debug.Log($"{evt.newValue} -> {evt.target}");
    }
}