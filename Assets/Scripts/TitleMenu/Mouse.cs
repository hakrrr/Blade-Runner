using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    public GameObject cam;
    float ColorAlpha = 0f;
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Button") {
            StartCoroutine(ColorChange(other.gameObject));
        }
        else if (other.gameObject.tag == "Button2")
        {
            StartCoroutine(ColorChange2(other.gameObject));
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Button")
        {
            ColorAlpha = 0;
            StopAllCoroutines();
            other.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        }
        else if (other.gameObject.tag == "Button2")
        {
            ColorAlpha = 0;
            StopAllCoroutines();
            other.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
        }
    }
    IEnumerator ColorChange(GameObject a)
    { 

        while (a.GetComponent<Image>().color.a <= 1f)
        {
            ColorAlpha +=0.125f/2;
            a.GetComponent<Image>().color = new Color(255,255,255,ColorAlpha);
            yield return new WaitForSeconds(0.125f);
        }
        a.GetComponent<Button>().onClick.Invoke();
        ColorAlpha = 0;
    }
    IEnumerator ColorChange2(GameObject a)
    {

        while (a.GetComponent<Image>().color.a <= 0.3f)
        {
            ColorAlpha += 0.125f / 8;
            a.GetComponent<Image>().color = new Color(255, 255, 255, ColorAlpha);
            yield return new WaitForSeconds(0.125f);
        }
        a.GetComponent<Button>().onClick.Invoke();
        ColorAlpha = 0;
    }

}
