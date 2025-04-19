using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCarController _playerCar;
    [SerializeField] private WheelTrail _wheelTrail;
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Engine Audio")]
    [SerializeField] private AudioSource _lowRPM;
    [SerializeField] private AudioSource _highRPM;
    [SerializeField] private AnimationCurve _engineLowCurve;
    [SerializeField] private AnimationCurve _engineHighCurve;

    [Header("Tire Audio")]
    [SerializeField] private AudioSource _tireScreech;
    [SerializeField] private List<AudioResource> _audioSources;

    [Header("Effects")]
    [SerializeField] private AudioSource _conePickup;

    private void OnEnable() { ConeSpawner.OnConeCollected += PlayConePickup; }
    private void OnDisable() { ConeSpawner.OnConeCollected -= PlayConePickup; }

    private void Update()
    {
        EngineSound();
        TireScreech();
    }

    private void EngineSound()
    {
        // 600 idle rpm, 6000 max rpm
        int rpm = Mathf.Max(600, Mathf.FloorToInt(_playerCar.Speed / 25 * 6000));
        float rpmFactor = Mathf.InverseLerp(600, 6000, rpm);
        _lowRPM.volume = _engineLowCurve.Evaluate(rpmFactor);
        _highRPM.volume = _engineHighCurve.Evaluate(rpmFactor);
        _lowRPM.pitch = 1 + rpmFactor;
        _highRPM.pitch = 1 + rpmFactor;
    }

    private void TireScreech()
    {
        if (!_wheelTrail.IsTrailActive()) { _tireScreech.Stop(); return; }
        if (_tireScreech.isPlaying) { return; }

        _tireScreech.resource = GetRandomTireScreech();
        _tireScreech.Play();
    }

    private AudioResource GetRandomTireScreech()
    {
        return _audioSources[Random.Range(0, _audioSources.Count)];
    }

    public void StopGameAudio()
    {
        _lowRPM.Stop();
        _highRPM.Stop();
        _tireScreech.resource = null;
    }

    public void StartGameAudio()
    {
        _lowRPM.Play();
        _highRPM.Play();
    }

    public void SetAudioMixerVolume(float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
    }

    private void PlayConePickup()
    {
        _conePickup.Play();
    }
}
