using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestingUI : MonoBehaviour {
    [SerializeField]
    TMP_Text text;
    private static TestingUI instance = null;

    public static TestingUI Instance { get { return instance; } }


    List<string> logging = new List<string>(12);


    public void Awake() {
        if (instance == null)
            instance = this;
    }

    public void Start() {
        Application.targetFrameRate = 30;
    }

    public void AddLogging(string s) {
        logging.Add(s + "\n");
        if (logging.Count >= 12)
            logging.RemoveAt(0);
        text.text = "";
        foreach (var str in logging)
            text.text += str;
    }
}
