using Assets;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public static string Name;
    [SerializeField] TMP_InputField nameTMP_InputField;
    [SerializeField] TextMeshProUGUI scoreText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        var dataJSON = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataJSON);
        if (data != null)
        {
            Camera.main.backgroundColor = SettingsManager.HexToColor(data.BackgroundColor);
            if (data.Scores != null)
            {
                var topScore = data.Scores.OrderByDescending(k => k.Score).FirstOrDefault();
                scoreText.text = "Best Score : " + topScore.Name + " : " + topScore.Score;
            }
        }
        else
        {
            PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(new PlayerData { BackgroundColor = SettingsManager.ColorToHex(Color.black) }));
        }
    }

    public void StartGame()
    {
        Name = nameTMP_InputField.text;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Scoreboard()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
         Application.Quit();
#endif
    }
}
