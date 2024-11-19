using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
using System;
using LitJson;
public class UserInfo : MonoBehaviour
{
    private WXUserInfoButton _button;
    private class CloudCallContainerData
    {
        public string action;
    }
    private void Awake()
    {
        WX.cloud.Init();
        // 初始化微信小游戏 SDK
        // WX.InitSDK(
        //     (code) =>
        //     {
        //         Debug.Log("InitSDK: " + code);

//                 // 获取微信小游戏的字体
//                 var fallbackFont = Application.streamingAssetsPath + "/Fz.ttf";
//                 WX.GetWXFont(
//                     fallbackFont,
//                     (wxFont) =>
//                     {
//                         if (!wxFont)
//                             return;
//
//                         this.font = wxFont;
//                         OnFontLoaded?.Invoke(wxFont);
//                     }
//                 );
//
//                 // 获取微信小游戏的系统信息
// #if UNITY_WEBGL && !UNITY_EDITOR
//                 WindowInfo = WX.GetWindowInfo();
//                 MenuButtonBoundingClientRect = WX.GetMenuButtonBoundingClientRect();
// #else
//                 WindowInfo = new WindowInfo { safeArea = new SafeArea() };
//                 MenuButtonBoundingClientRect = new ClientRect();
// #endif
        //     }
        // );
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetTimeout(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private readonly Action<WXUserInfoResponse> _onTap = (res) =>
    {
        var result = "onTap\n" + JsonMapper.ToJson(res);
        Debug.LogError("_onTap:[" + result+ "]");
    };
    IEnumerator SetTimeout(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Vector2 size = GameManager.Instance.detailsController.GetInitialButtonSize();
        // Vector2 position = GameManager.Instance.detailsController.GetButtonPosition(-1);
        // Debug.Log(position);
        // Debug.Log(size);
        // var windowInfo = GameManager.Instance.WindowInfo;
        // var canvasWidth = (int)(windowInfo.screenWidth * windowInfo.pixelRatio);
        // var canvasHeight = (int)(windowInfo.screenHeight * windowInfo.pixelRatio);
        // Debug.LogError("SetTimeout begin");
        // AuthorizeOption authorizeOption = new AuthorizeOption();
        // authorizeOption.scope = "scope.userInfo"; //{scope: "scope.userInfo"}
        // authorizeOption.success = (e) =>
        // {
        //     Debug.Log("success");
        // };
        // authorizeOption.fail = (e) =>{
        //     Debug.Log("fail");};
        // authorizeOption.complete = (e) =>{
        //     Debug.Log("complete");};
        //
        // WX.Authorize(authorizeOption);
        // _button = WX.CreateUserInfoButton(
        //     20,
        //     20,
        //     400,
        //     200,
        //     "en",
        //     false
        // );
        // _button.OnTap(_onTap);
        Debug.LogError("SetTimeout start");
        var data = new CloudCallContainerData()
        {
            action = "inc"
        };
        WX.cloud.CallContainer(
            new CallContainerParam()
            {
                config = new ICloudConfig()
                {
                    env = "prod-3gx247tmff4cba5a", // 云托管环境ID，通过创建云托管服务获取
                },
                path = "/api/count", // 服务路径
                header =
                    new
                        Dictionary<string, string>() // 设置请求的 header，header 中不能设置 Referer。content-type 默认为 application/json
                        {
                            { "X-WX-SERVICE", "dotnet-j199" }
                        },
                method = "POST", // HTTP 请求方法

                data = JsonMapper.ToJson(data), // 请求数据
                success = (res) =>
                {
                    WX.ShowModal(new ShowModalOption()
                    {
                        content = "Cloud CallContainer Success: " + JsonMapper.ToJson(res)
                    });
                },
                fail = (res) =>
                {
                    WX.ShowModal(new ShowModalOption()
                    {
                        content = "Cloud CallContainer Fail: " + JsonMapper.ToJson(res)
                    });
                },
                complete = (res) =>
                {
                    WX.ShowModal(new ShowModalOption()
                    {
                        content = "Cloud CallContainer Complete: " + JsonMapper.ToJson(res)
                    });
                }
            }
            // {
            //     // config: {
            //     //     "env": "prod-3gx247tmff4cba5a"
            //     // },
            //     path: "/api/count",
            //     header: {
            //         "X-WX-SERVICE": "dotnet-j199"
            //     },
            //     "method": "POST",
            //     "data": {
            //         "action": "inc"
            //     }
            // }
        );
        Debug.LogError("SetTimeout end");

    }
}
