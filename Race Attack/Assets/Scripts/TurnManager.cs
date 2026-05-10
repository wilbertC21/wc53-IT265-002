using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    
    [Header("Turn Settings")]
    public int totalPlayers = 4; // Set by PlayerCountUI
    public int currentPlayerIndex = 0;
    
    [Header("Turn Transition UI")]
    public GameObject turnTransitionPanel;
    public TextMeshProUGUI turnTransitionText;
    public float transitionDuration = 2f;
    
    [Header("Current Turn Display")]
    public TextMeshProUGUI currentPlayerText;
    
    public delegate void TurnChangedEvent(int newPlayerIndex);
    public event TurnChangedEvent OnTurnChanged;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        UpdateCurrentPlayerDisplay();
    }
    
    public void StartGame(int playerCount)
    {
        totalPlayers = playerCount;
        currentPlayerIndex = 0;
        ShowTurnTransition();
    }

    public void SetCurrentPlayer(int playerIndex)
{
    currentPlayerIndex = playerIndex;
    OnTurnChanged?.Invoke(currentPlayerIndex);
}
    
    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;
        OnTurnChanged?.Invoke(currentPlayerIndex);
        ShowTurnTransition();
    }
    
    private void ShowTurnTransition()
    {
        StartCoroutine(TurnTransitionRoutine());
    }
    
    private IEnumerator TurnTransitionRoutine()
    {
        // Show transition screen
        if (turnTransitionPanel != null)
        {
            turnTransitionPanel.SetActive(true);
            
            string playerName = GetCurrentPlayerName();
            turnTransitionText.text = $"{playerName}'s Turn";
            
            // Wait for transition duration
            yield return new WaitForSeconds(transitionDuration);
            
            // Hide transition screen
            turnTransitionPanel.SetActive(false);
        }
        
        UpdateCurrentPlayerDisplay();
    }
    
    private void UpdateCurrentPlayerDisplay()
    {
        if (currentPlayerText != null)
        {
            currentPlayerText.text = $"Current: {GetCurrentPlayerName()}";
        }
    }
    
    public string GetCurrentPlayerName()
    {
        // Boss is always player 0
        if (currentPlayerIndex == 0)
        {
            return "Boss";
        }
        else
        {
            return $"Racer {currentPlayerIndex}";
        }
    }
    
    public bool IsBossTurn()
    {
        return currentPlayerIndex == 0;
    }
    
    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }
}