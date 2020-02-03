using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuNew : MonoBehaviour {

	Animator CameraObject;
    public GameObject kinectmodel;
    public Transform kinectleftHand;
    public Transform kinectrightHand;
    public GameObject cursor;
    public Light light;
    public bool switchhand = true;

    void Update()
    {
        if(switchhand == false)
            cursor.transform.localPosition = Vector3.Lerp(cursor.transform.localPosition,new Vector3 (kinectleftHand.transform.position.x*20, kinectleftHand.transform.position.y*20, 16.24f),0.5f);
        else
        cursor.transform.localPosition = Vector3.Lerp(cursor.transform.localPosition, new Vector3(kinectrightHand.transform.position.x * 20, kinectrightHand.transform.position.y * 20, 16.24f), 0.5f);
    }

    [Header("Loaded Scene")]
	[Tooltip("The name of the scene in the build settings that will load")]
	public string sceneName = ""; 

	[Header("Panels")]
	[Tooltip("The UI Panel parenting all sub menus")]
	public GameObject mainCanvas;

	[Header("SFX")]
	[Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
	public GameObject hoverSound;
	[Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
	public GameObject sliderSound;
	[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
	public GameObject swooshSound;

	// campaign button sub menu
	[Header("Menus")]
	[Tooltip("The Menu for when the MAIN menu buttons")]
	public GameObject mainMenu;
	[Tooltip("The Menu for when the PLAY button is clicked")]
	public GameObject playMenu;
	[Tooltip("The Menu for when the EXIT button is clicked")]
	public GameObject exitMenu;
    public GameObject Calibration;

    public GameObject lefthand;
    public GameObject righthand;

    [Header("LOADING SCREEN")]
	public GameObject loadingMenu;
	public Slider loadBar;

	public TMP_Text finishedLoadingText;
    private SceneMg sceneManager;

    void Start(){
		CameraObject = transform.GetComponent<Animator>();
        sceneManager = GameObject.Find("SceneMg").GetComponent<SceneMg>();
	}

	public void  PlayCampaign (){
        sceneManager.FadeToScene(1);
	}
	
	public void  PlayCampaignMobile (){
		exitMenu.gameObject.SetActive(false);
		playMenu.gameObject.SetActive(true);
		mainMenu.gameObject.SetActive(false);
	}

	public void  ReturnMenu (){
		playMenu.gameObject.SetActive(false);
		exitMenu.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	public void NewGame(){
		if(sceneName != ""){
			StartCoroutine(LoadAsynchronously(sceneName));
			//SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
	}
    public void Position2()
    {
        transform.DORotate(Vector3.up * 90f, 0.5f);
    }
    public void Position1() {
        transform.DORotate(Vector3.zero, 0.5f);
    }
    public void Setright()
    {
        righthand.gameObject.SetActive(true);
        lefthand.gameObject.SetActive(false); 
        switchhand = true;
    }
    public void Setleft()
    {
        righthand.gameObject.SetActive(false);
        lefthand.gameObject.SetActive(true);
        switchhand = false;
    }

	public void PlayHover (){
		hoverSound.GetComponent<AudioSource>().Play();
	}

	public void PlaySFXHover (){
		sliderSound.GetComponent<AudioSource>().Play();
	}

	public void PlaySwoosh (){
		swooshSound.GetComponent<AudioSource>().Play();
	}

	// Are You Sure - Quit Panel Pop Up
	public void  AreYouSure (){
		exitMenu.gameObject.SetActive(true);
	}

	public void  AreYouSureMobile (){
		exitMenu.gameObject.SetActive(true);
		mainMenu.gameObject.SetActive(false);
	}

	public void Yes (){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
	}
    public void No()
    {
        exitMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
    public void ToCalibration()
    {
        kinectmodel.transform.rotation=new Quaternion (0,1,0, kinectmodel.transform.rotation.w);
        //kinectmodel.transform.localPosition = new Vector3(-50,0,0);

        light.transform.RotateAroundLocal(new Vector3(0, 1, 0), -90);
        Transform[] allChildren = kinectmodel.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
       
    }
    IEnumerator LoadAsynchronously (string sceneName){ // scene name is just the name of the current scene being loaded
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
			operation.allowSceneActivation = false;
			mainCanvas.SetActive(false);
			loadingMenu.SetActive(true);

			while (!operation.isDone){
				float progress = Mathf.Clamp01(operation.progress / .9f);
				loadBar.value = progress;

				if(operation.progress >= 0.9f){
					finishedLoadingText.gameObject.SetActive(true);

					if(Input.anyKeyDown){
						operation.allowSceneActivation = true;
					}
				}
				
				yield return null;
			}
		}
}