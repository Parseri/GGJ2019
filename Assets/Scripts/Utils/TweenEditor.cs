using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TweenEditor : MonoBehaviour {

    [Header ("Tween name")]
    new public string name = "";

    public bool autoPlay = false;

    [Header("Tween times")]
    public float waitTime = 0.25f;
    public float tweenTime = 2.0f;
    [Header("Tween types")]
    public LoopType loopType = LoopType.Yoyo;
    [Tooltip("use -1 for endless loop)")]
    public int LoopTimes = 0;
    public Ease easeType = Ease.Linear;

    //public virtual void TweenPlay() { }
   
	
}
