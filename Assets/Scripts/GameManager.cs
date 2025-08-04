using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public bool gameOver = false;

    // Tree variables
    public GameObject[] logs;
    public List<GameObject> logList;

    private float logHeight = 2.43f;
    private float initialYPosition = -4.42f;
    private int maxLogs = 6;
    private bool logWithoutBranch = false;

    // Score variables
    public Text scoreText;
    public Text level;
    private int score = 0;

    // Time variables
    public Image timeBar;
    private float timeBarWidth = 330f;

    private float gameDuration = 20f;
    private float extraTime = 0.20f; // Defines game level
    private float currentTime;

    void Awake() // Required because we have a static variable
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
        InitializeLogs();
        DefineDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            DecreaseBar();
            DefineDifficulty();
        }
    }

    void CreateLog(int position)
    {
        GameObject log = Instantiate(logWithoutBranch ? logs[Random.Range(0, 3)] : logs[0]);

        log.transform.localPosition = new Vector3(0f, initialYPosition + position * logHeight, 0f);

        logList.Add(log);

        logWithoutBranch = !logWithoutBranch;
    }

    void InitializeLogs()
    {
        for (int position = 0; position <= maxLogs; position++)
        {
            CreateLog(position);
        }
    }

    void CutLog()
    {
        // The cut log is always the first one (POSITION 0)
        Destroy(logList[0]);
        logList.RemoveAt(0);
        SoundManager.instance.PlaySound(SoundManager.instance.corteSound);
        AddScore();
        AddTime();
    }

    void RepositionLogs()
    {
        for (int position = 0; position < logList.Count; position++)
        {
            logList[position].transform.localPosition = new Vector3(0f, initialYPosition + position * logHeight, 0f);
        }
        CreateLog(maxLogs);
    }

    void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void DefineDifficulty()
    {
        switch (score)
        {
            case 0:
                level.text = "Level 1";
                StartCoroutine(HideLevel());
                break;

            case 20:
                level.gameObject.SetActive(true);
                extraTime = 0.15f;
                level.text = "Level 2";
                StartCoroutine(HideLevel());
                break;
            case 50:
                level.gameObject.SetActive(true);
                extraTime = 0.10f;
                level.text = "Level 3";
                StartCoroutine(HideLevel());
                break;
            case 80:
                level.gameObject.SetActive(true);
                extraTime = 0.05f;
                level.text = "Level 4";
                StartCoroutine(HideLevel());
                break;
            case 100:
                level.gameObject.SetActive(true);
                extraTime = 0.02f;
                level.text = "Level 5";
                StartCoroutine(HideLevel());
                break;
            case 120:
                level.gameObject.SetActive(true);
                extraTime = 0.01f;
                level.text = "Level 6";
                StartCoroutine(HideLevel());
                break;
            default:
                break;
        }
    }

    IEnumerator HideLevel()
    {
        yield return new WaitForSeconds(2);
        level.gameObject.SetActive(false);
    }

    void AddTime()
    {
        if (currentTime + extraTime < gameDuration) // Ensure we don't exceed max game time
        {
            currentTime += extraTime;
        }
    }

    void DecreaseBar()
    {
        currentTime -= Time.deltaTime;

        float ratio = currentTime / gameDuration;
        float position = timeBarWidth - (ratio * timeBarWidth);

        timeBar.transform.localPosition = new Vector2(-position, timeBar.transform.localPosition.y);

        if (currentTime <= 0) // Time's up
        {
            gameOver = true;
            SaveScore();
        }
    }

    public void SaveScore()
    {
        if (PlayerPrefs.GetInt("best") < score)
        {
            PlayerPrefs.SetInt("best", score);
        }

        PlayerPrefs.SetInt("score", score);

        SoundManager.instance.PlaySound(SoundManager.instance.dieSound);

        Invoke("ShowGameOver", 2f);
    }

    public void ShowGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Touch()
    {
        if (!gameOver)
        {
            CutLog();
            RepositionLogs();
        }
    }

}