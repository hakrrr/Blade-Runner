using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMg : MonoBehaviour
{
    static SceneMg curr;
    public Animator animator;
    private int SceneId;
    private int prevSceneId;

    private void Awake()
    {
        if (curr != null && curr != this)
            Destroy(gameObject);
        else
        {
            curr = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneSwitch;
    }
    public void FadeToScene(int id)
    {
        if(SceneId != id)
        {
            SceneId = id;
            prevSceneId = SceneManager.GetActiveScene().buildIndex;
            animator.SetTrigger("FadeOut");
        }
    }

    public void OnFadeOut()
    {
        SceneManager.LoadScene(SceneId);
    }
    private void OnSceneSwitch(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex != prevSceneId && scene.isLoaded)
            animator.SetTrigger("FadeIn");
        Time.timeScale = 1f;
    }

}
