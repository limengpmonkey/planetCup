using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBalls : MonoBehaviour
{
    [SerializeField]
    private GameObject cup;
    [SerializeField] private List<GameObject> _corlorballs_total;
    private BallData.Classify_total _classifytotal;
    private BallData.Classify_Compose _classifycompose;
    private readonly float _num = 0.02f;
    private int idx = 0;
    private bool isstop = false;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ExecuteAfterInterval", 0.6f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (cup.transform.childCount == 18 && !isstop)
        {
            isstop = true;
            CancelInvoke("ExecuteAfterInterval");
        }
    }
    void ExecuteAfterInterval()
    {
        idx %= 3;
        GameObject obj = Instantiate(_corlorballs_total[idx], cup.transform.position+Vector3.up, Quaternion.identity, cup.transform);
        idx++;
        StartCoroutine( Composealgorithm.Instance.ScaleOverTime(new Vector3(_num,_num,_num),obj));
        
        // 添加其他需要执行的操作或者调用其他函数
    }

}
