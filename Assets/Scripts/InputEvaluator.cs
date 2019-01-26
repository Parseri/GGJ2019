using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEvaluator : MonoBehaviour {
    public enum InputButton {
        UNDEFINED,
        LEFT,
        RIGHT,
        SUICIDE
    }

    public enum InputEvent {
        UNDEFINED,
        UP,
        DOWN
    }

    private static InputEvaluator instance = null;

    public static InputEvaluator Instance {
        get {
            return instance;
        }
    }
    public class InputSaver {
        public InputEvent ie;
        public InputButton ib;
        public float ts;

        public InputSaver(InputButton bu, InputEvent ev, float stamp) {
            ie = ev;
            ib = bu;
            ts = stamp;
        }
    }
    private List<InputSaver> inputs = new List<InputSaver>();
    private int replayIndex = -1;
    private float replayStartTs;
    private bool allFinished = false;
    private float originalStartTs = -1;
    public bool Replaying { get { return replayIndex >= 0 && !allFinished; } }
    public bool Ended { get { return allFinished; } }
    [SerializeField]
    private Image knob;
    private Vector3 original;

    void Awake() {
        if (instance == null) {
            instance = this;
            original = knob.transform.position;
        }
    }

    void OnDestroy() {
        if (instance == this) instance = null;
    }

    public void StartReplay() {
        replayIndex = 0;
        replayStartTs = Time.time;
        knob.transform.position = original;
        allFinished = false;
    }

    public void StartRecording() {
        originalStartTs = -1;
        replayIndex = -1;
        knob.transform.position = original;
        inputs.Clear();
        allFinished = false;
    }

    public void EvaluateInput(InputButton button, InputEvent ievent) {
        if (originalStartTs < 0)
            originalStartTs = Time.time;
        if (replayIndex >= 0) {
            if (replayIndex < inputs.Count) {
                InputSaver saver = inputs[replayIndex];
                var ts = Time.time - replayStartTs;
                Debug.Log("saver: " + replayIndex + " b: " + saver.ib + " e: " + saver.ie + " t: " + saver.ts + " other: " + ts);
                if (ts >= saver.ts) {
                    button = saver.ib;
                    ievent = saver.ie;
                    replayIndex++;
                    //TestingUI.Instance.AddLogging("button: " + button.ToString() + " event: " + ievent.ToString() + " ts: " + ts);
                } else
                    return;
            } else {
                allFinished = true;
                return;
            }
        } else {
            var ts = Time.time - originalStartTs;
            Debug.Log("button: " + button.ToString() + " event: " + ievent.ToString() + " ts: " + ts);
            //TestingUI.Instance.AddLogging("button: " + button.ToString() + " event: " + ievent.ToString() + " ts: " + ts);
            inputs.Add(new InputSaver(button, ievent, ts));
        }
        var newPos = knob.transform.position;
        if (ievent == InputEvent.DOWN) {
            if (button == InputButton.LEFT)
                newPos.x -= 10;
            if (button == InputButton.RIGHT)
                newPos.x += 10;
        }
        knob.transform.position = newPos;
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
