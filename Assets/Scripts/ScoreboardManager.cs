using Assets;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextPrefab;
    [SerializeField] GameObject scoreBoardContainer;

    void Start()
    {
        var dataJSON = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataJSON);
        Camera.main.backgroundColor = SettingsManager.HexToColor(data.BackgroundColor);

        if (data.Scores != null)
        {
            List<PlayerScores> scores = data.Scores.OrderByDescending(k => k.Score).ToList();
            int index = 1;
            foreach (PlayerScores score in scores)
            {
                var a = Instantiate(scoreTextPrefab, scoreBoardContainer.transform);
                a.GetComponent<TextMeshProUGUI>().text = index + "." + score.Name + " : " + score.Score;
                index++;
            }
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
