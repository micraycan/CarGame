using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private TextMeshProUGUI _timerField;
    [SerializeField] private TextMeshProUGUI _highScoreField;
    [SerializeField] private TextMeshProUGUI _scoreField;
    [SerializeField] private Slider _volumeSlider;

    public float Volume => _volumeSlider.value;

    private void OnEnable() => ConeSpawner.OnConeCollected += UpdateScore;
    private void OnDisable() => ConeSpawner.OnConeCollected -= UpdateScore;

    private void Awake()
    {
        _highScoreField.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        _scoreField.text = "0";
        _volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
    }

    private void Update()
    {
        if (GameManager.Instance.Timer > 0)
        {
            _timerField.text = $"0:0{GameManager.Instance.Timer.ToString("0")}";
        }
    }

    public void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);
        _timerPanel.SetActive(false);
        _mainMenuPanel.SetActive(false);
    }

    public void ShowTimer()
    {
        _timerPanel.SetActive(true);
        _gameOverPanel.SetActive(false);
        _mainMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _timerPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
    }

    public void UpdateScore()
    {
        if (GameManager.Instance.IsGameActive)
        {
            GameManager.Instance.Score++;
            _scoreField.text = GameManager.Instance.Score.ToString();

            if (GameManager.Instance.Score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", GameManager.Instance.Score);
                PlayerPrefs.Save();
                _highScoreField.text = GameManager.Instance.Score.ToString();
            }
        }
    }
}