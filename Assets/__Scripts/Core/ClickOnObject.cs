using UnityEngine;
using System.Collections.Generic;

public class ClickOnObject : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]
    private GameObject tag2prefab;
    [SerializeField]
    private GameObject cup;
    public Vector3 targetScale = Vector3.one * 0.08f; // Ŀ���С
    //public float rotationSpeed = 100.0f; // ��ת�ٶ�
    public float sensitivity = 0.1f; // ��ת������
    private Vector2 touchStartPos;



    private void Start()
    {
        Application.targetFrameRate = 30;
        //StartCoroutine(ScaleOverTime(targetScale));
        mainCamera = Camera.main;
    }

    void Update()
    {

        if (Input.touchCount > 0) // ����д����¼�
        {
            Touch touch = Input.GetTouch(0); // ��ȡ��һ��������

            if (touch.phase == TouchPhase.Began) // ������ʼ
            {
                touchStartPos = touch.position; // ��¼��ʼλ��
            }
            else if (touch.phase == TouchPhase.Moved) // �����ƶ���
            {
                Vector2 touchEndPos = touch.position; // ��ȡ�ƶ��е�λ��
                float deltaX = touchEndPos.x - touchStartPos.x; // ����X����Ĳ�ֵ

                // ��ת����
                cup.transform.Rotate(Vector3.up * -deltaX * sensitivity, Space.World);

                touchStartPos = touchEndPos; // ���¿�ʼλ��
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //float mouseX = Input.GetAxis("Mouse X");

            // ���ݻ���ֵ��ת����
            //transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);

            int ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
            LayerMask layerMask = ~(1 << ignoreLayer);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
                var gameobj = hit.collider.gameObject;
                string tagname = gameobj.tag;
                Composealgorithm.Instance.ChoseFlow(gameobj,tagname);
                // if (!Trytest(in gameobj, tagname))
                // {
                //     Composealgorithm.Instance.ClearSelectedState();
                // }
                // else
                // {
                //     BallEffect.LightTheBall(gameobj);
                //
                // }
            }
        }
    }

    // bool Trytest(in GameObject gameObject, string tagname)
    // {
    //
    //     // BallData.Classify_total color;
    //     // Composealgorithm.Instance.StrParseBasicClassify(tagname,out color);
    //     // Debug.LogError("Trytest : "+color);
    //     // if (Composealgorithm.Instance.IsSelected(gameObject)) return true;
    //     //
    //     // if (!Composealgorithm.Instance.PushInSelectedList(gameObject)) return false;
    //
    //     return true;
    // }


}