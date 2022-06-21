using Assets;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreTextUp;
    public Text ScoreTextDown;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        var dataJSON = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataJSON);
        Camera.main.backgroundColor = SettingsManager.HexToColor(data.BackgroundColor);

        if (data.Scores != null)
        {
            var topScore = data.Scores.OrderByDescending(k => k.Score).FirstOrDefault();
            ScoreTextUp.text = "Best Score : " + topScore.Name + " : " + topScore.Score;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreTextDown.text = $"Score : {m_Points}";

    }

    public void GameOver()
    {
        var dataJSON = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataJSON);

        if (data.Scores == null)
        {
            data.Scores = new List<PlayerScores>();
            data.Scores.Add(new PlayerScores
            {
                Name = MenuManager.Name,
                Score = m_Points,
            });
            PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(data));
        }
        else
        {
            PlayerScores score = data.Scores.FirstOrDefault(k => k.Name == MenuManager.Name);
            if (score == null)
            {
                data.Scores.Add(new PlayerScores
                {
                    Name = MenuManager.Name,
                    Score = m_Points,
                });
            }
            else
            {
                data.Scores.Remove(score);
                data.Scores.Add(new PlayerScores
                {
                    Name = MenuManager.Name,
                    Score = m_Points,
                });
            }
            PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(data));
        }

        var topScore = data.Scores.OrderByDescending(k => k.Score).FirstOrDefault();
        ScoreTextUp.text = "Best Score : " + topScore.Name + " : " + topScore.Score;

        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
