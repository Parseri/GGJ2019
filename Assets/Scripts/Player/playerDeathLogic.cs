using UnityEngine;
using System.Collections;
using PC2D;
using UnityEngine.SceneManagement;

public class playerDeathLogic : MonoBehaviour {

    public Vector3 respawnTo;
    private int currentCheckpointNum = -1;
    private PlatformerAnimation2D animationLogic;
    public bool dying = false;

    [SerializeField]
    private GameObject DeathEffect;

    void Start() {
        animationLogic = GetComponent<PlatformerAnimation2D>();
    }

    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag.Equals("Enemy") && dying == false) {
           
            GameObject parent = GameObject.FindGameObjectWithTag("SplatParent");
            this.GetComponent<PlayerSplatterLogic>().SpawnChunkParticles(this.transform.position, Vector3.up, parent.transform);
            this.GetComponent<PlayerSplatterLogic>().Splatt(this.transform.position, other);
            this.GetComponent<PlatformerMotor2D>().enabled = false;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            die();
        }

        if (other.tag.Equals("End") && dying == false) {
            InputSystem.Instance.updateTime = false;
            InputSystem.Instance.SetLastTime();
            NextScene();
        }

    }

    public void die() {
        dying = true;
        this.GetComponent<PlayerController2D>().enabled = false;
        restartCurrentScene();
       // AutoFade.FadeOut(Color.black, restartCurrentScene);	
    }

    public void restartCurrentScene() {
        StartCoroutine(ResetGame());
    }

    public void NextScene() {
        StartCoroutine(NextScenetus());
    }

    private IEnumerator NextScenetus() {
        this.transform.GetChild(0).gameObject.SetActive(false);
        InputSystem.Instance.ResetSystem();
        yield return new WaitForSecondsRealtime(1);
        GameObject.Destroy(GameObject.Find("InputSystem"));
        int scene = SceneManager.GetActiveScene().buildIndex+1;
        if (scene >= SceneManager.sceneCountInBuildSettings) scene = 0;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    private IEnumerator ResetGame() {
        this.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(2);
        InputSystem.Instance.StartLevel();
    }

    private IEnumerator ReloadScene() {
        this.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}