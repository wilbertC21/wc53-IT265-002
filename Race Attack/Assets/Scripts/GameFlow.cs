using UnityEngine;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }
    
    [Header("References")]
    public GameObject[] racerCars; // Assign all racer cars (Purple, Red, Blue, Gray)
    public GameObject bossCar; // monster_car_07
    
    [Header("Game State")]
    private int currentDiceRoll;
    private bool waitingForDiceRoll = false;
    
    private enum GamePhase
    {
        CardPhase,
        DicePhase,
        ActionPhase
    }
    
    private GamePhase currentPhase = GamePhase.CardPhase;
    
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
        // Subscribe to events
        ActionSelectionUI.OnActionSelected += HandleActionSelected;
        Dice.OnDiceResult += HandleDiceResult;
    }
    
    private void OnDestroy()
    {
        ActionSelectionUI.OnActionSelected -= HandleActionSelected;
        Dice.OnDiceResult -= HandleDiceResult;
    }
    
    private void Update()
    {
        // In dice phase, wait for Space key
        if (currentPhase == GamePhase.DicePhase && waitingForDiceRoll)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed! Rolling dice NOW!");
                RollDice();
            }
        }
    }
    
    // Called when player clicks "Check Match" and has a match
    public void OnCardMatchFound()
    {
        Debug.Log($"{TurnManager.Instance.GetCurrentPlayerName()} matched! Press SPACE to roll!");
        
        // Hide card UI
        if (CardGameUI.Instance != null)
        {
            CardGameUI.Instance.HideCardPhase();
        }

        // Reset dice from previous turn
    if (DiceThrow.Instance != null)
    {
        DiceThrow.Instance.ResetDice();
    }
        
        // Start dice phase
        currentPhase = GamePhase.DicePhase;
        waitingForDiceRoll = true;
    }
    
    private void RollDice()
    {
        waitingForDiceRoll = false;
        
        Debug.Log("Rolling dice...");
        
        // Trigger dice throw
        if (DiceThrow.Instance != null)
        {
            DiceThrow.Instance.RollSingleDice();
        }
    }
    
    private void HandleDiceResult(int diceIndex, int faceValue)
    {
        currentDiceRoll = faceValue;
        
        Debug.Log($"Dice result: {faceValue}");
        
        // Wait a moment for dice to settle, then show actions
        StartCoroutine(ShowActionsAfterDelay(1f));
    }
    
    private IEnumerator ShowActionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        currentPhase = GamePhase.ActionPhase;

        DiceFollowCamera diceCamera = FindObjectOfType<DiceFollowCamera>();
        if (diceCamera != null)
            {
        diceCamera.ReturnHome();
            }
        
        // Show action selection UI
        if (ActionSelectionUI.Instance != null)
        {
            ActionSelectionUI.Instance.ShowActions(currentDiceRoll);
        }
    }
    
    private void HandleActionSelected(string actionType, int value)
    {
        int currentPlayerIndex = TurnManager.Instance.GetCurrentPlayerIndex();
        
        Debug.Log($"Executing {actionType} action for Player {currentPlayerIndex} with value {value}");
        
        switch (actionType)
        {
            case "Move":
                ExecuteMoveAction(currentPlayerIndex, value);
                break;
            case "Attack":
                ExecuteAttackAction(value);
                break;
            case "Block":
                ExecuteBlockAction(currentPlayerIndex, value);
                break;
        }
        
        // After action completes, go to next turn
        StartCoroutine(NextTurnAfterDelay(1.5f));
    }
    
    private void ExecuteMoveAction(int playerIndex, int moveAmount)
    {
        GameObject car = GetCarForPlayer(playerIndex);
        
        if (car != null)
        {
            CarMover mover = car.GetComponent<CarMover>();
            if (mover == null)
            {
                mover = car.AddComponent<CarMover>();
            }
            
            mover.MoveForward(moveAmount);
            
            // Boss gains +1 HP when racer moves (per GDD)
            if (playerIndex != 0 && BossHealth.Instance != null)
            {
                BossHealth.Instance.Heal(1);
                Debug.Log("Boss gained +1 HP because racer moved!");
            }
        }
    }
    
    private void ExecuteAttackAction(int damage)
    {
        int amplifiedDamage = damage * 5;
        if (BossHealth.Instance != null)
        {
            BossHealth.Instance.TakeDamage(amplifiedDamage);
        }
        else
        {
            Debug.LogWarning("BossHealth not found!");
        }
    }
    
    private void ExecuteBlockAction(int playerIndex, int blockAmount)
    {
        // For prototype: block the next racer in turn order
        int targetPlayerIndex = (playerIndex + 1) % TurnManager.Instance.totalPlayers;
        
        // Don't let racers block the boss for now
        if (targetPlayerIndex == 0)
        {
            targetPlayerIndex = (targetPlayerIndex + 1) % TurnManager.Instance.totalPlayers;
        }
        
        GameObject targetCar = GetCarForPlayer(targetPlayerIndex);
        
        if (targetCar != null)
        {
            CarMover mover = targetCar.GetComponent<CarMover>();
            if (mover == null)
            {
                mover = targetCar.AddComponent<CarMover>();
            }
            
            mover.MoveBackward(blockAmount);
            Debug.Log($"Player {playerIndex} blocked Player {targetPlayerIndex} - moved back {blockAmount} pixels!");
        }
    }
    
    private GameObject GetCarForPlayer(int playerIndex)
    {
        if (playerIndex == 0)
        {
            return bossCar; // Boss is player 0
        }
        else
        {
            int racerIndex = playerIndex - 1; // Racers are players 1, 2, 3...
            if (racerIndex >= 0 && racerIndex < racerCars.Length)
            {
                return racerCars[racerIndex];
            }
        }
        
        return null;
    }
    
    private IEnumerator NextTurnAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    
    // Check if all players have gone (end of round)
    int nextPlayer = (TurnManager.Instance.GetCurrentPlayerIndex() + 1) % TurnManager.Instance.totalPlayers;
    
    if (nextPlayer == 0) // Back to player 0 means round is over
    {
        Debug.Log("=== ROUND COMPLETE ===");
        StartNewRound();
    }
    
    // Go to next turn
    currentPhase = GamePhase.CardPhase;
    TurnManager.Instance.NextTurn();
    
    // AFTER advancing turn, check if new current player is Boss and if Boss is dead
    int currentPlayer = TurnManager.Instance.GetCurrentPlayerIndex();
    
    if (currentPlayer == 0 && BossHealth.Instance != null && BossHealth.Instance.IsDead())
    {
        Debug.Log("Boss is dead! Skipping to next player.");
        // Recursively skip to next turn
        StartCoroutine(NextTurnAfterDelay(0f));
        yield break; // Exit this coroutine
    }
    
    // Show card phase for next player
    if (CardGameUI.Instance != null)
    {
        CardGameUI.Instance.ShowCardPhase();
    }
}
    
    private void StartNewRound()
    {
        // Draw new goal card
        if (CardDeck.Instance != null)
        {
            CardDeck.Instance.DrawNewGoalCard();
        }
        
        // Reshuffle and redeal cards
        if (CardGameUI.Instance != null)
        {
            CardGameUI.Instance.InitializeHands(TurnManager.Instance.totalPlayers);
        }
        
        Debug.Log("New round started! New Goal Card drawn.");
    }
}