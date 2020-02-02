using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMg : MonoBehaviour
{
    static SceneMg curr;
    public Animator animator;
    private int SceneId;

    private void Awake()
    {
        if (curr != null && curr != this)
            Destroy(this);
        curr = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneSwitch;
    }
    public void FadeToScene(int id)
    {
        SceneId = id;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeFinished()
    {
        SceneManager.LoadScene(SceneId);
    }
    private void OnSceneSwitch(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex != 0 && scene.isLoaded)
            animator.SetTrigger("FadeIn");
    }
}
