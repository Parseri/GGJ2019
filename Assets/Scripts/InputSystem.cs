using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour {

    void FixedUpdate() {
        if (Input.GetKeyUp(KeyCode.A)) {
            InputEvaluator.Instance.StartReplay();
        }
        if (InputEvaluator.Instance.Replaying) {
            InputEvaluator.Instance.EvaluateInput(InputEvaluator.InputButton.UNDEFINED, InputEvaluator.InputEvent.UNDEFINED);
        } else {
            if (Input.GetMouseButtonDown(0)) {
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
