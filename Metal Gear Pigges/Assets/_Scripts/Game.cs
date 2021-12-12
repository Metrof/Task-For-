using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Header("Set in inspector")]
    public Image DeathScreen;
    public Image WinScreen;
    public GameObject bigBoomPref;
    public GameObject SoundManager;
    public Farmer farmer;
    public List<Point> plantPoints;
    public AudioClip loseClip;
    public AudioClip winClip;
    public AudioClip plantingClip;

    [Header("Set Dinamically")]
    private bool gameOver = false;

    private AudioSource adSou;

    private void Awake()
    {
        farmer.points = plantPoints;
        adSou = SoundManager.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < plantPoints.Count; i++)
        {
            if (!plantPoints[i].plant) return;
        }
        Invoke("Win", 0);
    }
    void Win()
    {
        WinScreen.gameObject.SetActive(true);
        for (int i = 0; i < plantPoints.Count; i++)
        {
            plantPoints[i].plant = false;
        }
        farmer.Loose();
        GameObject bigBoom = Instantiate(bigBoomPref);
        bigBoom.transform.position = Vector3.zero;
        Destroy(bigBoom, 5f);
        adSou.PlayOneShot(winClip, 0.7F);
        adSou.clip = winClip;
        gameOver = true;
    }
    void Planting()
    {
        adSou.PlayOneShot(plantingClip);
    }
    void Loose()
    {
        DeathScreen.gameObject.SetActive(true);
        gameOver = true;
        adSou.PlayOneShot(loseClip, 0.7F);
        adSou.clip = loseClip;
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
