using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private PlayerCarController _player;
    [SerializeField] private TrafficController _trafficController;
    [SerializeField] private ConeSpawner _coneSpawner;
    [SerializeField] private float _timeLimit;
    [SerializeField] private float _timeStepDown;
    private float _maxTimeLimit;
    private bool _isGameActive;

    public bool IsGameActive => _isGameActive;
    public Action OnGameStart;
    public int Score;

    public float Timer { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        _maxTimeLimit = _timeLimit;
        _isGameActive = false;
        GoToMainMenu();
    }

    private void Start()
    {
        _audioManager.SetAudioMixerVolume(PlayerPrefs.GetFloat("MasterVolume", 0f));
    }

    private void Update()
    {
        if (!_isGameActive) { return; }
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            EndGame();
        }
    }

    public void ResetTimer()
    {
        _timeLimit -= _timeStepDown;
        Timer = _timeLimit;
    }

    public void EndGame()
    {
        _isGameActive = false;
        Score = 0;
        _uiManager.ShowGameOver();
        _audioManager.StopGameAudio();
        _player.StopCar();
        _trafficController.StopTraffic();
        _coneSpawner.ClearCones();
    }

    public void StartGame()
    {
        if (_isGameActive) { return; }
        _uiManager.UpdateScore();
        _timeLimit = _maxTimeLimit;
        _isGameActive = true;
        OnGameStart?.Invoke();
        _uiManager.ShowTimer();
        _audioManager.StartGameAudio();
        _player.ResetCar();
        _trafficController.StartTraffic();
        _coneSpawner.Reset();
        Timer = _maxTimeLimit;
    }

    public void GoToMainMenu()
    {
        _uiManager.ShowMainMenu();
    }

    public void SetMasterAudio()
    {
        _audioManager.SetAudioMixerVolume(_uiManager.Volume);
        PlayerPrefs.SetFloat("MasterVolume", _uiManager.Volume);
        PlayerPrefs.Save();
    }
}
