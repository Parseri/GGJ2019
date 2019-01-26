using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using DG.Tweening;
[ExecuteInEditMode]

[RequireComponent(typeof(Image))]

public class FaderController : MonoBehaviour {
    [SerializeField]
    private Color Fade;

    void Awake() {
        onFade();
    }

    public void onFade() {
        this.GetComponent<Image>().DOColor(Fade,1f);
    }
}
