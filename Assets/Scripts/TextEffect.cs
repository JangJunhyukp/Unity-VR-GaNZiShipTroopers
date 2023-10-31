using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    Text text;
    int btnclick = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine("FadeOut", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn(float time)
    {
        Color color = text.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / time;
            text.color = color;
            yield return null;
        }

        StartCoroutine("FadeOut", 1.5f);
    }

    IEnumerator FadeOut(float time)
    {
        Color color = text.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / time;
            text.color = color;
            yield return null;
        }

        StartCoroutine("FadeIn", 1.5f);
    }

    public void Changecolor()
    {
        if (btnclick == 0)
        {
            StopAllCoroutines();
            text.color = Color.yellow;
            btnclick++;
        }

        else if(btnclick == 1)
        {
            text.color = Color.white;
            StartCoroutine("FadeOut", 2f);
            btnclick--;
        }
    }
}
