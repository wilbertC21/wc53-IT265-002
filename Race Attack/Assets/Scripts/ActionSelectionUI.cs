using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ActionSelectionUI : MonoBehaviour
{
    public static ActionSelectionUI Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject actionPanel;
    public Button moveButton;
    public Button attackButton;
    public Button blockButton;
    public TextMeshProUGUI diceResultText;
    
    private int currentDiceRoll;
    
    public static event Action<string, int> OnActionSelected; // action type, dice value
    
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
        // Setup button listeners
        moveButton.onClick.AddListener(() => SelectAction("Move"));
        attackButton.onClick.AddListener(() => SelectAction("Attack"));
        blockButton.onClick.AddListener(() => SelectAction("Block"));
        
        // Hide panel at start
        HideActions();
    }
    
    public void ShowActions(int diceRoll)
    {
        currentDiceRoll = diceRoll;
        
        if (diceResultText != null)
        {
            diceResultText.text = $"You rolled: {diceRoll}";
        }
        
        actionPanel.SetActive(true);
        Time.timeScale = 0f; // Pause while choosing
        
        Debug.Log($"Action selection shown. Dice roll: {diceRoll}");
    }
    
    public void HideActions()
    {
        actionPanel.SetActive(false);
        Time.timeScale = 1f; // Unpause
    }
    
    private void SelectAction(string actionType)
    {
        Debug.Log($"Player selected: {actionType} with value {currentDiceRoll}");
        
        HideActions();
        
        OnActionSelected?.Invoke(actionType, currentDiceRoll);
    }
}