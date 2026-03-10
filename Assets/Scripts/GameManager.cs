using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Ingame, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Menu;

    public event Action<GameState> OnStateChanged;

    [SerializeField] private PlayerController player;
    [SerializeField] private SegmentSpawner segmentSpawner;
    [SerializeField] private CameraFollow cameraFollow;

    private Vector3 playerStartPos;
    private Quaternion playerStartRot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (player != null)
        {
            playerStartPos = player.transform.position;
            playerStartRot = player.transform.rotation;
        }

        if (segmentSpawner != null && player != null)
            segmentSpawner.SetPlayer(player.transform);

        if (cameraFollow != null && player != null)
            cameraFollow.SetTarget(player.transform);

        SetState(GameState.Menu);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;

        bool playing = (newState == GameState.Ingame);

        if (player != null)
            player.SetCanMove(playing);

        Time.timeScale = playing ? 1f : 0f;

        OnStateChanged?.Invoke(newState);
    }

    public void Play()
    {
        if (cameraFollow != null)
            cameraFollow.SnapToTarget();

        SetState(GameState.Ingame);
    }

    public void Die() => SetState(GameState.GameOver);

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (player != null)
            player.ResetPlayer(playerStartPos, playerStartRot);

        if (cameraFollow != null)
            cameraFollow.SnapToTarget();

        if (segmentSpawner != null)
            segmentSpawner.ResetAndBuild();

        SetState(GameState.Ingame);
    }
}