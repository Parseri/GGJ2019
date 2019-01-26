using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputSystem : MonoBehaviour {
    private State recordState = State.Undefined;
    private enum State {
        Undefined,
        Recording,
        Replaying,
        Ended
    }

    private bool CanStartRecording { get { return recordState == State.Undefined || recordState == State.Ended; } }

    void FixedUpdate() {
        if (Input.GetKeyUp(KeyCode.S) && CanStartRecording) {
            recordState = State.Recording;
            TestingUI.Instance.AddLogging("state: " + recordState.ToString());
            InputEvaluator.Instance.StartRecording();
        }
        if (recordState == State.Undefined || recordState == State.Ended)
            return;
        if (Input.GetKeyUp(KeyCode.A)) {
            InputEvaluator.Instance.StartReplay();
            recordState = State.Replaying;
            TestingUI.Instance.AddLogging("state: " + recordState.ToString());
        }
        if (InputEvaluator.Instance.Replaying) {
            InputEvaluator.Instance.EvaluateInput(InputEvaluator.InputButton.UNDEFINED, InputEvaluator.InputEvent.UNDEFINED);
        } else if (InputEvaluator.Instance.Ended) {
            recordState = State.Ended;
            TestingUI.Instance.AddLogging("state: " + recordState.ToString());
        } else {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
                InputEvaluator.InputButton button = Input.mousePosition.x >= Screen.width * 0.5f ? InputEvaluator.InputButton.RIGHT : InputEvaluator.InputButton.LEFT;
                InputEvaluator.Instance.EvaluateInput(button, InputEvaluator.InputEvent.DOWN);
            }
            if (Input.GetMouseButtonUp(0)) {
                InputEvaluator.InputButton button = Input.mousePosition.x >= Screen.width * 0.5f ? InputEvaluator.InputButton.RIGHT : InputEvaluator.InputButton.LEFT;
                InputEvaluator.Instance.EvaluateInput(button, InputEvaluator.InputEvent.UP);
            }
            if (Input.touchCount > 0) {
                foreach (var touch in Input.touches) {
                    InputEvaluator.InputButton button = touch.position.x >= Screen.width * 0.5f ? InputEvaluator.InputButton.RIGHT : InputEvaluator.InputButton.LEFT;
                    switch (touch.phase) {
                        case TouchPhase.Began:
                            InputEvaluator.Instance.EvaluateInput(button, InputEvaluator.InputEvent.DOWN);
                            break;
                        case TouchPhase.Ended:
                            InputEvaluator.Instance.EvaluateInput(button, InputEvaluator.InputEvent.UP);
                            break;
                        case TouchPhase.Canceled:
                            InputEvaluator.Instance.EvaluateInput(button, InputEvaluator.InputEvent.UP);
                            break;
                    }
                }
            }
        }
    }
}
