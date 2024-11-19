using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class spawnBallsInLogin : MonoBehaviour
{
    [SerializeField]
    private GameObject cup;
    [SerializeField] private List<GameObject> _corlorballs_total;
    private BallData.Classify_total _classifytotal;
    private BallData.Classify_Compose _classifycompose;
    private readonly float _num = 0.02f;
    private bool isstop = false;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ExecuteAfterInterval", 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        // cup.transform.Rotate(Vector3.up * -deltaX * sensitivity, Space.World);
        cup.transform.Rotate(Vector3.up * -Time.deltaTime*50, Space.World);
        if (cup.transform.childCount >= 40)
        {
            for (int i = 0; i < cup.transform.childCount; i++)
            {
                //todo: youhua
                if (cup.transform.GetChild(i).transform.position.y < -10f)
                {
                    Destroy(cup.transform.GetChild(i).gameObject);
                }
            }
        }
    }
    void ExecuteAfterInterval()
    {
        GameObject obj = Instantiate(_corlorballs_total[Random.Range(0,_corlorballs_total.Count)], cup.transform.position + Vector3.up, Quaternion.identity, cup.transform);

        StartCoroutine( Composealgorithm.Instance.ScaleOverTime(new Vector3(_num,_num,_num),obj));
        
        // 添加其他需要执行的操作或者调用其他函数
    }

    void OnDestroy()
    {
        CancelInvoke("ExecuteAfterInterval");
    }

    private void OnDisable()
    {
        CancelInvoke("ExecuteAfterInterval");
    }

    private void OnEnable()
    {
        InvokeRepeating("ExecuteAfterInterval", 0.1f, 0.1f);
    }
}
