using Assets;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public static Color BackgroundColor;

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
        Camera.main.backgroundColor = HexToColor(data.BackgroundColor);
    }

    public static void SetBackgroundColor(int index)
    {
        var dataJSON = PlayerPrefs.GetString("PlayerData");
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataJSON);

        switch (index)
        {
            case 0:
                BackgroundColor = Color.black;
                break;
            case 1:
                BackgroundColor = Color.grey;
                break;
            case 2:
                BackgroundColor = Color.cyan;
                break;
        }

        data.BackgroundColor = ColorToHex(BackgroundColor);
        PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(data));
        Camera.main.backgroundColor = HexToColor(data.BackgroundColor);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public static string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}
