using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using DG.Tweening;
[ExecuteInEditMode]

public class DoTweenHelper : TweenEditor {

    [System.Serializable]
    public class OnStart : UnityEvent { };
    public OnStart onStart;

    [System.Serializable]
    public class OnComplete : UnityEvent { };
    public OnComplete onComplete;

    private TweenParams tParmsMove;
    private TweenParams tParmsScale;
    private TweenParams tParmsRotate;

    public void onTweenTo(Transform Moveto) {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        tParmsMove = new TweenParams().SetDelay(this.waitTime).SetLoops(this.LoopTimes, this.loopType).SetEase(this.easeType).OnStart(onStartCallback).OnComplete(onCompleteCallback).SetId(this.name).SetAutoKill(true);
        tParmsScale = new TweenParams().SetDelay(this.waitTime).SetLoops(this.LoopTimes, this.loopType).SetEase(this.easeType).SetId(this.name).SetAutoKill(true);
        tParmsRotate = new TweenParams().SetDelay(this.waitTime).SetLoops(this.LoopTimes, this.loopType).SetEase(this.easeType).SetId(this.name).SetAutoKill(true);
        onPlayTween(Moveto);
    }

    void onPlayTween(Transform Moveto) {

        if (Moveto.transform.localScale != this.transform.localScale) {
            this.transform.DOScale(Moveto.transform.localScale, this.tweenTime).SetAs(tParmsScale);
        }
        if (Moveto.transform.rotation != this.transform.rotation && this.GetComponent<LevelObjectController>().AutoRotate == false) {
            this.transform.DORotateQuaternion(Moveto.transform.rotation, this.tweenTime).SetAs(tParmsRotate);
        }

        this.transform.DOMove(Moveto.position, this.tweenTime).SetAs(tParmsMove);
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