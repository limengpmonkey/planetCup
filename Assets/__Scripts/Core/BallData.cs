using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData : MonoBehaviour
{

    [SerializeField] private Classify_total ballClassify;
    [SerializeField] private SummaryClassify summary;
    public enum Classify_total
    {
        red,
        green,
        blue,
        redblue,
        greenred,
        bluegreen,
        white,
        unknown
    };

    public enum Classify_Compose
    {
        redblue,
        greenred,
        bluegreen,
    }

    public enum Classify_Final
    {
        white
    }

    public enum Classify_basic
    {
        red,
        green,
        blue,
    }

    public enum SummaryClassify
    {
        None,
        Basic,
        Compose,
        Final
    }
}
