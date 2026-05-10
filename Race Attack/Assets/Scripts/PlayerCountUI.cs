using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCountUISimple : MonoBehaviour
{
    [Header("UI References")]
    public GameObject playerCountPanel;
    public TMP_Dropdown playerCountDropdown;
    public Button startButton;
    public TextMeshProUGUI titleText;
    
    [Header("Game Reference")]
    public DiceThrow diceThrow;
    
    [Header("Player Cars")]
    public GameObject[] playerCars; // Assign all 5 car GameObjects here
    
    private void Start()
    {
        ShowPlayerSelection();
        
        playerCountDropdown.ClearOptions();
        playerCountDropdown.AddOptions(new System.Collections.Generic.List<string> 
        { 
            "3 Players", 
            "4 Players", 
            "5 Players" 
        });
        
        playerCountDropdown.value = 0; // Default to 3 players
        
        startButton.onClick.AddListener(OnStartButtonClicked);
    }
    
    private void ShowPlayerSelection()
    {
        playerCountPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private void OnStartButtonClicked()
{
    int playerCount = playerCountDropdown.value + 3;
    
    Debug.Log($"Starting game with {playerCount} players");
    
   /* if (diceThrow != null)
    {
        diceThrow.amountOfDice = playerCount;
    }
    */
    
    SetActiveCars(playerCount);
    
    // Initialize turn system
    if (TurnManager.Instance != null)
    {
        TurnManager.Instance.StartGame(playerCount);
    }
    
    // Initialize card system
    if (CardDeck.Instance != null)
    {
        CardDeck.Instance.InitializeDeck();
    }
    
    if (CardGameUI.Instance != null)
    {
        CardGameUI.Instance.InitializeHands(playerCount);
        CardGameUI.Instance.ShowCardPhase();
    }
    
    playerCountPanel.SetActive(false);
    Time.timeScale = 1f;
}
    private void SetActiveCars(int playerCount)
    {
        if (playerCars == null || playerCars.Length == 0)
        {
            Debug.LogWarning("No player cars assigned!");
            return;
        }
        
        for (int i = 0; i < playerCars.Length; i++)
        {
            if (playerCars[i] != null)
            {
                playerCars[i].SetActive(i < playerCount);
                
                if (i < playerCount)
                {
                    Debug.Log($"Car {i + 1} enabled");
                }
                else
                {
                    Debug.Log($"Car {i + 1} disabled");
                }
            }
        }
    }
}