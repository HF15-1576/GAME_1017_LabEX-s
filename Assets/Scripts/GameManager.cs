using UnityEngine;

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
=======
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Ingame, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Menu;

    // This is for UI and other elements to react to state changes if needed
    public event Action<GameState> OnStateChanged;

    // References to Other Scripts
    [SerializeField] private PlayerController player;
    [SerializeField] private SegmentSpawner segmentSpawner;
    [SerializeField] private CameraFollow cameraFollow;

    private Vector3 playerStartPos;
    private Quaternion playerStartRot;

    private void Awake()
>>>>>>> Stashed changes
    {
        
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
=======
        // Keeps track of players position and rotation for easy restart
        if (player != null)
        {
            playerStartPos = player.transform.position;
            playerStartRot = player.transform.rotation;
        }

        // Set player reference in SegmentSpawner so it can manage spawning based on player position
        if (segmentSpawner != null)
        {
            segmentSpawner.SetPlayer(player.transform);
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

        if (cameraFollow != null)
            cameraFollow.SnapToTarget();

        if (segmentSpawner != null)
            segmentSpawner.ResetAndBuild();

        SetState(GameState.Ingame);
>>>>>>> Stashed changes
    }

}
