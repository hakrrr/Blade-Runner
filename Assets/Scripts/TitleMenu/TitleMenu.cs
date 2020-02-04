using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleMenu : MonoBehaviour {

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

	[Header("Menus")]
	public GameObject mainMenu;
	public GameObject exitMenu;
    public GameObject Calibration;
	public GameObject loadingMenu;
	public Slider loadBar;

	public TMP_Text finishedLoadingText;
    private SceneMg sceneManager;
    private CursorScript cursor;

    void Start(){
        sceneManager = GameObject.Find("SceneMg").GetComponent<SceneMg>();
        cursor = GameObject.Find("HandCursor").GetComponent<CursorScript>();
    }

    #region OnClickEvent
    public void  PlayCampaign (){
        sceneManager.FadeToScene(1);
	}
	public void  ReturnMenu (){
		exitMenu.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}
	public void NewGame(){
		if(sceneName != ""){
			StartCoroutine(LoadAsynchronously(sceneName));
		}
	}
    public void Position2()
    {
        transform.DORotate(Vector3.up * 90f, 0.5f);
        cursor.transform.DORotate(Vector3.up * 90f, 0.5f);
        cursor.screenDist = 25f;
    }
    public void Position1() {
        transform.DORotate(Vector3.zero, 0.5f);
        cursor.transform.DORotate(Vector3.zero, 0.5f);
        cursor.screenDist = 20f;
    }
	public void PlayHover(){
		hoverSound.GetComponent<AudioSource>().Play();
	}
	public void PlaySFXHover(){
		sliderSound.GetComponent<AudioSource>().Play();
	}
	public void PlaySwoosh(){
		swooshSound.GetComponent<AudioSource>().Play();
	}
	public void AreYouSure(){
		exitMenu.SetActive(true);
        mainMenu.SetActive(false);
        cursor.locked = false;
    }
    public void Yes(){
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
        cursor.locked = false;
    }
    #endregion
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