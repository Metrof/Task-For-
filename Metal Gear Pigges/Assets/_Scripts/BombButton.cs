using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombButton : MonoBehaviour
{
    float bombColldown = 5f;
    float colldownDone;
    float colldownStart;
    Button button;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    public void Reload()
    {
        button.interactable = false;
        image.fillAmount = 0;
        colldownStart = Time.time;
        colldownDone = Time.time + bombColldown;
    }

    private void Update()
    {
        if (colldownStart == 0) return;

        float u = (Time.time - colldownStart) / bombColldown;
        if (Time.time >= colldownDone)
        {
            button.interactable = true;
            image.fillAmount = 1;
            colldownStart = 0;
            return;
        }
        image.fillAmount = u;
    }
}
