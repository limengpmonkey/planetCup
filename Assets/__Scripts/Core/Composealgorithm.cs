using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Composealgorithm : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _selectedballs = new List<GameObject>();

    [SerializeField] private GameObject cup;
    private BallData.SummaryClassify currentSummaryClassify = BallData.SummaryClassify.None;
    private static Composealgorithm instance;
    private readonly uint _maxcount = 2;

    [SerializeField] private GameObject redblue;
    [SerializeField] private GameObject greenred;
    [SerializeField] private GameObject bluegreen;
    [SerializeField] private GameObject white;
    
    public float duration = 2.0f; // change time s
    private readonly float _composenum = 0.04f;
    private int maxcount = 3;
    
    public delegate void EventDelegate();   // 定义委托类型
 
    public static EventDelegate EventUpdateCoin;    // 更新Coin事件的委托代理
    private float composeMixColorSecond = 0.5f;
    public static Composealgorithm Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Composealgorithm>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Composealgorithm");
                    instance = obj.AddComponent<Composealgorithm>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public bool IsSelected(in GameObject whichball)
    {
        return _selectedballs.Contains(whichball);
    }
    
    public bool PushInSelectedList(in GameObject whichball)
    {
        if (_selectedballs.Count == maxcount) return false;
        _selectedballs.Add(whichball);
        return true;
    }
    
    public int SelectedCount()
    {
        return _selectedballs.Count;
    }
    public void ClearSelectedState()
    {
        foreach (var selectedball in _selectedballs)
        {
            BallEffect.UnLightTheBall(selectedball);
        }
        _selectedballs.Clear();
    }

    public bool StrParseBasicClassify(string tagname,out BallData.Classify_basic color)
    {
        
        if (BallData.Classify_basic.TryParse(tagname, out color))
        {
            Debug.Log("StrParseBasicClassify success: "+tagname);
            return true;
        }
        else
        {
            Debug.LogError("StrParseBasicClassify error: "+tagname);
            return false;
        }
    }
    public bool StrParseComposeClassify(string tagname,out BallData.Classify_Compose  color)
    {
        
        if (BallData.Classify_Compose.TryParse(tagname, out color))
        {
            Debug.Log("StrParseComposeClassify success: "+tagname);
            return true;
        }
        else
        {
            Debug.LogError("StrParseComposeClassify error: "+tagname);
            return false;
        }
    }
    
    public bool StrParseFinalClassify(string tagname,out BallData.Classify_Final  color)
    {
        
        if (BallData.Classify_Final.TryParse(tagname, out color))
        {
            Debug.Log("StrParseComposeClassify success: "+tagname);
            return true;
        }
        else
        {
            Debug.LogError("StrParseComposeClassify error: "+tagname);
            return false;
        }
    }

    public void ChoseFlow(in GameObject ball, string name)
    {
        if (IsSelected(ball)) return;
        if (SelectedCount() == 0)
        {
            currentSummaryClassify = tagname2Summary(name);
            PushInSelectedList(ball);
            BallEffect.LightTheBall(ball);
        }else if (SelectedCount() >= 1)
        {
            var summarythisBall = tagname2Summary(name);
            /*
             * 同级basic 可随意合并升级到下一个Compose状态,可入list
             * 同级compose,需要选中三个完全不同的球才可以合并
             * 同级final,需要三个完全一致的白球才可以消除
             */
            if (currentSummaryClassify != summarythisBall)
            {
                UnlightAllSelectedBalls();
                ClearSelectedState();
                currentSummaryClassify = summarythisBall;
                BallEffect.LightTheBall(ball);
                PushInSelectedList(ball);
                EventUpdateCoin();
                return;
            }

            if (currentSummaryClassify == summarythisBall)
            {
                switch (currentSummaryClassify)
                {
                    case BallData.SummaryClassify.Basic:
                    {
                        BasicBallAlgorithm(ball,name); //just one
                    }
                        break;
                    case BallData.SummaryClassify.Compose:
                    {
                        ComposeBallAlgorithm(ball,name); //the most is two
                    }
                        break;
                    case BallData.SummaryClassify.Final:
                    {
                        FinalBallAlgorithm(ball, name); //just two
                    }
                        break;
                    default:
                        break;
                }
            }
            
            
        }
        EventUpdateCoin();
        Debug.LogWarning("_selectedballs.Count["+_selectedballs.Count+"]");
    }

    private BallData.SummaryClassify tagname2Summary(string name)
    {
        BallData.SummaryClassify result = BallData.SummaryClassify.None;
        if (StrParseBasicClassify(name, out _))
        {
            result = BallData.SummaryClassify.Basic;
        }

        if (StrParseComposeClassify(name, out _))
        {
            result = BallData.SummaryClassify.Compose;
        }

        if (StrParseFinalClassify(name, out _))
        {
            result = BallData.SummaryClassify.Final;
        }

        return result;
    }

    private void UnlightAllSelectedBalls()
    {
        foreach (var bSelectedball in _selectedballs)
        {
            BallEffect.UnLightTheBall(bSelectedball);
        }
    }

    private void BasicBallAlgorithm(GameObject ball, string name)
    {
        Debug.Log("_selectedBalls Number: "+_selectedballs.Count.ToString());
        GameObject incontainerBall = _selectedballs[0];
        string incontainerBallTag = incontainerBall.tag;
        if (incontainerBallTag == name)
        {
            UnlightAllSelectedBalls();
            ClearSelectedState();
            BallEffect.LightTheBall(ball);
            PushInSelectedList(ball);
            return;
        }
        PushInSelectedList(ball);
        EventUpdateCoin();

        System.Collections.IEnumerator ComposeMixColor()
        {
            yield return new WaitForSeconds(composeMixColorSecond);
            string composenameA = name + incontainerBallTag;
            string composenameB = incontainerBallTag + name;
            
            Debug.Log("composenameA : " + composenameA);
            Debug.Log("composenameB : " + composenameB);
            
            BallData.Classify_Compose compose;
            //find compose ball name 
            if (!StrParseComposeClassify(composenameA, out compose))
            {
                StrParseComposeClassify(composenameB, out compose);
            }
            
            //创建小球
            var composeobj = Instantiate(SwitchComposePrefab(compose), ball.transform.position, Quaternion.identity,cup.transform);
            StartCoroutine(Composealgorithm.Instance.ScaleOverTime(new Vector3(_composenum, _composenum, _composenum), composeobj));
            //该消除原来的小球了-->(建议慢慢变小后删除 :TODO)
            DestoryBallInContainer();
            PushInSelectedList(composeobj);
            currentSummaryClassify = BallData.SummaryClassify.Compose;
            BallEffect.LightTheBall(composeobj);
            EventUpdateCoin();
            // yield return null;
        }

        StartCoroutine(ComposeMixColor());

    }

    private void ComposeBallAlgorithm(GameObject ball, string name)
    {
        //要是三个不同的小球才能够合并,否则直接抛弃原来已经缓存的小球
        List<string> tagList = new List<string>();
        foreach (var composeball in _selectedballs)
        {
            tagList.Add(composeball.tag);
        }

        if (tagList.Contains(name))
        {
            UnlightAllSelectedBalls();
            ClearSelectedState();
            BallEffect.LightTheBall(ball);
            PushInSelectedList(ball);
            return;
        }

        PushInSelectedList(ball);
        BallEffect.LightTheBall(ball);
        EventUpdateCoin();
        System.Collections.IEnumerator FinalMix()
        {
            if (_selectedballs.Count == 3)
            {
                yield return new WaitForSeconds(composeMixColorSecond);
                var biggestBall = Instantiate(white, ball.transform.position, Quaternion.identity, cup.transform);
                StartCoroutine(Composealgorithm.Instance.ScaleOverTime(new Vector3(_composenum, _composenum, _composenum), biggestBall));
                foreach (var composeball in _selectedballs)
                {
                    Destroy(composeball);
                }
                ClearSelectedState();
                BallEffect.LightTheBall(biggestBall);
                PushInSelectedList(biggestBall);
                currentSummaryClassify = BallData.SummaryClassify.Final;
                EventUpdateCoin();
            }

            yield return null;
        }
        StartCoroutine(FinalMix());


    }

    private void FinalBallAlgorithm(in GameObject ball, string name)
    {
        if (_selectedballs.Count < 3)
        {
            PushInSelectedList(ball);
            BallEffect.LightTheBall(ball);
            EventUpdateCoin();
        }
        
        if (_selectedballs.Count == 3)
        {
            
            System.Collections.IEnumerator ClearFinals()
            {
                yield return new WaitForSeconds(composeMixColorSecond);
                foreach (var wball in _selectedballs)
                {
                    Destroy(wball);
                }
                ClearSelectedState();
                currentSummaryClassify = BallData.SummaryClassify.None;
                EventUpdateCoin();
            }

            StartCoroutine(ClearFinals());
        }
    }
    private GameObject SwitchComposePrefab(BallData.Classify_Compose compose)
    {
        GameObject prefab = null;
        switch (compose)
        {
            case BallData.Classify_Compose.bluegreen:
            {
                prefab = bluegreen;
            }
                break;
            case BallData.Classify_Compose.redblue:
            {
                prefab = redblue;
            }
                break;
            case BallData.Classify_Compose.greenred:
            {
                prefab = greenred;
            }
                break;
        }

        return prefab;
    }
    
    public System.Collections.IEnumerator ScaleOverTime(Vector3 endScale, GameObject gameobj)
    {
        float elapsedTime = 0.0f;
        Vector3 startScale = gameobj.transform.localScale;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; 
            t = Mathf.SmoothStep(0.0f, 1f, t);
            gameobj.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
    }

    private void DestoryBallInContainer()
    {
        foreach (var ball in _selectedballs)
        {
            Destroy(ball);
        }
        ClearSelectedState();
    }

    public BallData.SummaryClassify CurrentSummaryClassify()
    {
        return currentSummaryClassify;
    }
    
    public List<GameObject> GetSelectedBalls()
    {
        return _selectedballs;
    } 
}
