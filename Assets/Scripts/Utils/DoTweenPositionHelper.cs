using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using DG.Tweening;
[ExecuteInEditMode]

public class DoTweenPositionHelper : TweenEditor {

    [System.Serializable]
    public class OnStart : UnityEvent { };
    public OnStart onStart;

    [System.Serializable]
    public class OnComplete : UnityEvent { };
    public OnComplete onComplete;

    private TweenParams tParms;

    public void onTweenTo(Transform Moveto) {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        tParms = new TweenParams().SetDelay(this.waitTime).SetLoops(this.LoopTimes, this.loopType).SetEase(this.easeType).OnStart(onStartCallback).OnComplete(onCompleteCallback).SetId(this.name);
        onPlayTween(Moveto);  
    }

    void onPlayTween(Transform Moveto) {
        this.transform.DOMove(Moveto.position, this.tweenTime).SetAs(tParms);
    }

    void onCompleteCallback() {
        if (onComplete != null)
            onComplete.Invoke();
    }

    void onStartCallback() {
        if (onStart != null)
            onStart.Invoke();
    }
}
