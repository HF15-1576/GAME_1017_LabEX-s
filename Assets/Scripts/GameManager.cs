using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Ingame, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Menu;

    // This is for UI and other elements to react to state changes if needed
    public event Action<GameState> OnStateChanged;

    // References to Other Scripts
    [SerializeField] private PlayerController player;

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
        // Keeps track of players position and rotation for easy restart
        if (player != null)
        {
            playerStartPos = player.transform.position;
            playerStartRot = player.transform.rotation;
        }

        SetState(GameState.Menu);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;

        // Freeze/unfreeze gameplay
        bool playing = (newState == GameState.Ingame);

        if (player != null)
            player.SetCanMove(playing);

        Time.timeScale = playing ? 1f : 0f;

        OnStateChanged?.Invoke(newState);
    }

    // UI calls these:
    public void Play() => SetState(GameState.Ingame);

    public void Die() => SetState(GameState.GameOver);

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Reset player
        if (player != null)
            player.ResetPlayer(playerStartPos, playerStartRot);


        SetState(GameState.Ingame);
    }
}
