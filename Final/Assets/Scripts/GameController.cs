using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public AudioSource Music;
    public AudioClip bgClip;
    public AudioClip winClip;
    public AudioClip lossClip;

    public Text ScoreText;
    public Text restartText;
    public Text gameOverText;
    public Text winText;
    public Text creditText;
    public Text modeText;

    private bool hardMode;
    private bool gameOver;
    private bool restart;
    internal int score;

    void Start()
    {
        gameOver = false;
        restart = false;
        hardMode = false;
        restartText.text = "";
        gameOverText.text = "";
        winText.text = "";
        creditText.text = "";
        modeText.text = "Press R to enable Hard Mode.";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
        GetComponent<AudioSource>();
        Music.clip = bgClip;
        Music.Play();
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hardMode = true;
            modeText.text = "Hard mode begins next wave. Good luck!";
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'Space' for Restart";
                modeText.text = "";
                restart = true;
                break;
            }
            
            if (hardMode)
            {
                modeText.text = "";
                hazardCount = 20;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
        if (score >= 100)
        {
            winText.text = "You win!";
            creditText.text = "GAME CREATED BY: EMILY RADNEY";
            gameOver = true;
            restart = true;
            Music.clip = winClip;
            Music.Play();
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";

        gameOver = true;
        if (score < 100)
        {
            Music.clip = lossClip;
            Music.Play();
        }
    }
}
