using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public bool gameOver = false;

    // Tree Variables

    public GameObject[] trunks;
    public List<GameObject> trunkList;

    private float trunkHeight = 2.43f;
    private float initialPositionY = -4.41f;
    private int maxTrunks = 6;
    private bool branchlessTrunk = false;

    // Score Variables
    public Text scoreText;
    public Text levelText;
    private int points = 0;

    // Time Variables
    public Image timeBar;
    private float timeBarWidth = 330f;

    private float gameDuration = 20f;
    private float extraTime = 0.20f; // Set game level
    private float currentTime;

    void Awake() // Necessary because of a static variable
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = gameDuration;
        InitializeTrunks();
        SetDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            DecreaseTimeBar();
            SetDifficulty();
        }
    }

    void CreateTrunks(int position)
    {

        GameObject trunk = Instantiate(branchlessTrunk ? trunks[UnityEngine.Random.Range(0, 3)] : trunks[0]);

        trunk.transform.localPosition = new Vector3(0f, initialPositionY + position * trunkHeight, 0f);

        trunkList.Add(trunk);

        branchlessTrunk = !branchlessTrunk;
    }

    void InitializeTrunks()
    {
        for (int position = 0; position <= maxTrunks; position++)
        {
            CreateTrunks(position);
        }
    }

    void CutTrunk()
    {
        // The cut trunk will always be the first one (POSITION 0)
        Destroy(trunkList[0]);
        trunkList.RemoveAt(0);
        SoundManager.instance.PlaySound(SoundManager.instance.cutSound);
        IncreasePoints();
        IncreaseTime();
    }

    void RepositionTrunk()
    {
        for (int position = 0; position < trunkList.Count; position++)
        {
            trunkList[position].transform.localPosition = new Vector3(0f, initialPositionY + position * trunkHeight, 0f);
        }
        CreateTrunks(maxTrunks);
    }

    void IncreasePoints()
    {
        points++;
        scoreText.text = points.ToString();
    }

    void SetDifficulty()
    {
        switch (points)
        {
            case 0:
                levelText.text = "Level 1";
                StartCoroutine(DestroyText());
                break;

            case 20:
                levelText.gameObject.SetActive(true);
                extraTime = 0.15f;
                levelText.text = "Level 2";
                StartCoroutine(DestroyText());
                break;
            case 50:
                levelText.gameObject.SetActive(true);
                extraTime = 0.10f;
                levelText.text = "Level 3";
                StartCoroutine(DestroyText());
                break;
            case 80:
                levelText.gameObject.SetActive(true);
                extraTime = 0.08f;
                levelText.text = "Level 4";
                StartCoroutine(DestroyText());
                break;
            case 100:
                levelText.gameObject.SetActive(true);
                extraTime = 0.04f;
                levelText.text = "Level 5";
                StartCoroutine(DestroyText());
                break;
            case 140:
                levelText.gameObject.SetActive(true);
                extraTime = 0.02f;
                levelText.text = "Level 6";
                StartCoroutine(DestroyText());
                break;
            case 180:
                levelText.gameObject.SetActive(true);
                extraTime = 0.015f;
                levelText.text = "Unlimited";
                StartCoroutine(DestroyText());
                break;

            default:
                break;
        }
    }

    IEnumerator DestroyText()
    {
        yield return new WaitForSeconds(2);
        levelText.gameObject.SetActive(false);
    }

    void IncreaseTime()
    {
        if (currentTime + extraTime < gameDuration) // Check if it won't exceed the maximum game time
        {
            currentTime += extraTime;
        }
    }

    void DecreaseTimeBar()
    {
        currentTime = currentTime - Time.deltaTime;

        float time = currentTime / gameDuration;
        float position = timeBarWidth - (time * timeBarWidth);

        timeBar.transform.localPosition = new Vector2(-position, timeBar.transform.localPosition.y);

        if (currentTime <= 0) // Time is up
        {
            gameOver = true;
            SaveScore();
        }
    }

    public void SaveScore()
    {
        if (PlayerPrefs.GetInt("best") < points)
        {
            PlayerPrefs.SetInt("best", points);
        }

        PlayerPrefs.SetInt("score", points);

        SoundManager.instance.PlaySound(SoundManager.instance.dieSound);

        Invoke("LoadGameOverScene", 2f);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Touch()
    {
        if (!gameOver)
        {
            CutTrunk();
            RepositionTrunk();
        }
    }
}
