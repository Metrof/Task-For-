using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public bool plant = false;
    public GameObject bombPreffab;
    private GameObject plantBomb;
    private SpriteRenderer sRend;

    private void Start()
    {
        plantBomb = Instantiate(bombPreffab);
        plantBomb.transform.position = transform.position;
        plantBomb.transform.SetParent(transform);
        sRend = plantBomb.GetComponent<SpriteRenderer>();
        sRend.enabled = false;
    }

    private void FixedUpdate()
    {
        if (plant)
        {
            sRend.enabled = true;
            return;
        }
        sRend.enabled = false;
    }
}
