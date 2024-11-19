using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect : MonoBehaviour
{

    public static bool LightTheBall(in GameObject ball)
    {
        var mat = ball.GetComponent<MeshRenderer>().material;
        if (!mat) return false;
        mat.SetFloat("_Highlighted",1);
        return true;
    }
    public static bool UnLightTheBall(in GameObject ball)
    {
        var mat = ball.GetComponent<MeshRenderer>().material;
        if (!mat) return false;
        mat.SetFloat("_Highlighted",0);
        return true;
    }
}
