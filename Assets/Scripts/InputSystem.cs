using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSystem : MonoBehaviour {
    [SerializeField]
    private GameObject playerObjectPrefab;
    [SerializeField]
    private GameObject simulatedObjectPrefab;
    private bool runState = false;

    public Text levelTimeText;
    public bool updateTime = false;
    public float startTime;

    private static InputSystem instance;

    public static InputSystem Instance { get { return instance; } }

    private List<InputEvaluator> evaluators;

    private InputEvaluator playerEvaluator;

    private bool CanStartRecording { get { return !runState; } }
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void OnDestroy() {
        if (instance == this) instance = null;
    }

    public void Start() {
        DontDestroyOnLoad(this);
        ResetSystem();
        Debug.Log("Started by System");
        StartLevel();
    }

    public void ResetSystem() {
        evaluators = new List<InputEvaluator>();
        playerEvaluator = null;
        Debug.Log("Reset by System");
        startTime = Time.timeSinceLevelLoad;
        updateTime = true;
    }

    public void StartLevel() {
        if (playerEvaluator != null) {
            evaluators.Add(new InputEvaluator(playerEvaluator));
            Debug.Log("New evaluator");
        }
        playerEvaluator = new InputEvaluator();
        playerEvaluator.SetController(playerObjectPrefab);
        Debug.Log("playerEvaluator == null: " + (playerEvaluator == null));
        foreach (var ev in evaluators) {
            ev.SetController(simulatedObjectPrefab);
        }
    }

    void Update() {
        if (updateTime && levelTimeText != null) {
            var levelTime = Time.timeSinceLevelLoad - startTime;
            levelTimeText.text = levelTime.ToString("F2");
        }
        if (playerEvaluator == null) return;
        if (!playerEvaluator.started) playerEvaluator.StartRecording();
        playerEvaluator.ResetMovement();
        foreach (var ev in evaluators) {
            ev.EvaluateInput(InputEvaluator.InputButton.UNDEFINED, InputEvaluator.InputEvent.UNDEFINED);
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1)) {
            playerEvaluator.EvaluateInput(InputEvaluator.InputButton.JUMP, InputEvaluator.InputEvent.DOWN);
        }
        if (Input.GetMouseButtonUp(0)) {
            playerEvaluator.EvaluateInput(InputEvaluator.InputButton.JUMP, InputEvaluator.InputEvent.UP);
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {// || Input.GetMouseButton(0)) {
            InputEvaluator.InputButton button = Input.mousePosition.x >= Screen.width * 0.5f ? InputEvaluator.InputButton.RIGHT : InputEvaluator.InputButton.LEFT;
            playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.DOWN);
        }
        if (Input.GetMouseButtonUp(0)) {
            InputEvaluator.InputButton button = Input.mousePosition.x >= Screen.width * 0.5f ? InputEvaluator.InputButton.RIGHT : InputEvaluator.InputButton.LEFT;
            playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.UP);
        }
#else
        if (Input.touchCount > 0) {
            foreach (var touch in Input.touches) {
                InputEvaluator.InputButton button = touch.position.x >= Screen.width * 0.5f ? InputEvaluator.InputButton.RIGHT : InputEvaluator.InputButton.LEFT;
                switch (touch.phase) {
                    case TouchPhase.Began:
                        playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.DOWN);
                        break;
                    case TouchPhase.Stationary:
                        playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.DOWN);
                        break;
                    case TouchPhase.Moved:
                        playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.DOWN);
                        break;
                    case TouchPhase.Ended:
                        playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.UP);
                        break;
                    case TouchPhase.Canceled:
                        playerEvaluator.EvaluateInput(button, InputEvaluator.InputEvent.UP);
                        break;
                }
            }
        }
#endif
    }
}

