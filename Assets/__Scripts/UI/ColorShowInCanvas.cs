using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorShowInCanvas : MonoBehaviour
{
    private UIDocument _gameroot;
    private VisualElement _Basic;
    private VisualElement _Compose;
    private VisualElement _Final;
    private VisualElement _colorcanvas;

    private Dictionary<BallData.SummaryClassify, VisualElement> classify2Element =
        new Dictionary<BallData.SummaryClassify, VisualElement>();
    private Dictionary<string, Color> name2color =
        new Dictionary<string, Color>();
    private Dictionary<string, VisualElement> name2Circle =
        new Dictionary<string, VisualElement>();

    private Dictionary<BallData.SummaryClassify, StyleScale> name2Scale = new Dictionary<BallData.SummaryClassify, StyleScale>();
    private BallData.SummaryClassify CanvasCurrentSummary = BallData.SummaryClassify.None;
    private void OnEnable()
    {
        _gameroot = GetComponent<UIDocument>();
        _Basic = _gameroot.rootVisualElement.Q<VisualElement>("Basic");
        _Compose = _gameroot.rootVisualElement.Q<VisualElement>("Compose");
        _Final = _gameroot.rootVisualElement.Q<VisualElement>("Final");
        _colorcanvas = _gameroot.rootVisualElement.Q<VisualElement>("colorcanvas");
        
        Composealgorithm.EventUpdateCoin += UpdateCircles;
        
        classify2Element.Add(BallData.SummaryClassify.Basic,_Basic);
        classify2Element.Add(BallData.SummaryClassify.Compose,_Compose);
        classify2Element.Add(BallData.SummaryClassify.Final,_Final);
        //--------------------------------------
        name2color.Add("red",Color.red);
        name2color.Add("blue",Color.blue);
        name2color.Add("green",Color.green);
        name2color.Add("redblue",Color.magenta);
        name2color.Add("greenred",Color.yellow);
        name2color.Add("bluegreen",Color.cyan);
        name2color.Add("white_0",Color.white);
        name2color.Add("white_1",Color.white);
        name2color.Add("white_2",Color.white);
        //---------------------------------------
        name2Circle.Add("red",_gameroot.rootVisualElement.Q<VisualElement>("red"));
        name2Circle.Add("blue",_gameroot.rootVisualElement.Q<VisualElement>("blue"));
        name2Circle.Add("green",_gameroot.rootVisualElement.Q<VisualElement>("green"));
        name2Circle.Add("redblue",_gameroot.rootVisualElement.Q<VisualElement>("redblue"));
        name2Circle.Add("greenred",_gameroot.rootVisualElement.Q<VisualElement>("greenred"));
        name2Circle.Add("bluegreen",_gameroot.rootVisualElement.Q<VisualElement>("bluegreen"));
        name2Circle.Add("white_0",_gameroot.rootVisualElement.Q<VisualElement>("white_0"));
        name2Circle.Add("white_1",_gameroot.rootVisualElement.Q<VisualElement>("white_1"));
        name2Circle.Add("white_2",_gameroot.rootVisualElement.Q<VisualElement>("white_2"));
        //---------------------------------------
        name2Scale.Add(BallData.SummaryClassify.Basic,_Basic.style.scale);
        name2Scale.Add(BallData.SummaryClassify.Compose,_Compose.style.scale);
        name2Scale.Add(BallData.SummaryClassify.Final,_Final.style.scale);

        _Basic.style.scale = new StyleScale(0);
        _Compose.style.scale = new StyleScale(0);
        _Final.style.scale = new StyleScale(0);
    }

    private void OnDisable()
    {
        Composealgorithm.EventUpdateCoin -= UpdateCircles;
        classify2Element.Clear();
        name2color.Clear();
        name2Circle.Clear();
        name2Scale.Clear();
    }

    private void UpdateCircles()
    {
        Debug.LogError(Enum.GetName(typeof(BallData.SummaryClassify),Composealgorithm.Instance.CurrentSummaryClassify()));
        if (CanvasCurrentSummary != Composealgorithm.Instance.CurrentSummaryClassify())
        {
            if (CanvasCurrentSummary != BallData.SummaryClassify.None) 
                classify2Element[CanvasCurrentSummary].style.scale = new StyleScale(0);
            foreach (var item in classify2Element)
            {
                if (item.Key == Composealgorithm.Instance.CurrentSummaryClassify())
                {
                    item.Value.style.display = DisplayStyle.Flex;
                }
                else
                {
                    item.Value.style.display = DisplayStyle.None;
                }

                foreach (var child in item.Value.Children())
                {
                    child.style.backgroundColor = Color.clear;
                    child.style.display = DisplayStyle.None;
                }
            }
            
            if (Composealgorithm.Instance.CurrentSummaryClassify() != BallData.SummaryClassify.None )
                classify2Element[Composealgorithm.Instance.CurrentSummaryClassify()].style.scale = name2Scale[Composealgorithm.Instance.CurrentSummaryClassify()];
        }

        int index = 0;
        foreach (var ball in Composealgorithm.Instance.GetSelectedBalls())
        {
            Debug.Log("UpdateCircles : [" + ball.tag + "]");
            // if (CanvasCurrentSummary != Composealgorithm.Instance.CurrentSummaryClassify())
            // name2Circle[ball.tag].style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            if (Composealgorithm.Instance.CurrentSummaryClassify() == BallData.SummaryClassify.Final)
            {
                string finalname = ball.tag + "_" + index.ToString();
                name2Circle[finalname].style.backgroundColor = new StyleColor(name2color[finalname]);
                name2Circle[finalname].style.display = DisplayStyle.Flex;
            }
            else
            {
                name2Circle[ball.tag].style.backgroundColor = new StyleColor(name2color[ball.tag]);
                name2Circle[ball.tag].style.display = DisplayStyle.Flex;
            }
            index++;
        }


        CanvasCurrentSummary = Composealgorithm.Instance.CurrentSummaryClassify();
    }
}
