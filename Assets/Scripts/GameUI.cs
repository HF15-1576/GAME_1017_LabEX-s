using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        var state = GameManager.Instance.CurrentState;

        if (menuPanel != null) menuPanel.SetActive(state == GameManager.GameState.Menu);
        if (gameOverPanel != null) gameOverPanel.SetActive(state == GameManager.GameState.GameOver);
    }
}
