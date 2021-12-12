using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject explPreff;

    public void Explousion()
    {
        GameObject pointToExplo = Instantiate(explPreff);
        pointToExplo.transform.position = transform.position;
        Destroy(gameObject);
        Destroy(pointToExplo, 0.35f);
    }
}
