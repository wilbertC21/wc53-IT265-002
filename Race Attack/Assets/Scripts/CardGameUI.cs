
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardGameUI : MonoBehaviour
{
    public static CardGameUI Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject cardHandPanel;
    public Transform cardContainer;
    public GameObject cardPrefab;
    
    [Header("Goal Card")]
    public TextMeshProUGUI goalCardText;
    public TextMeshProUGUI goalDescriptionText;
    
    [Header("Actions")]
    public Button passCardButton;
    public Button drawCardButton;
    public Button checkMatchButton;
    
    [Header("Info")]
    public TextMeshProUGUI deckCountText;
    
    private List<GameObject> currentCardUI = new List<GameObject>();
    private List<PlayerHand> allPlayerHands = new List<PlayerHand>();
    private int selectedCardIndex = -1;
    
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
        passCardButton.onClick.AddListener(OnPassCard);
        drawCardButton.onClick.AddListener(OnDrawCard);
        checkMatchButton.onClick.AddListener(OnCheckMatch);
        
        // Subscribe to turn changes
        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.OnTurnChanged += OnTurnChanged;
        }
    }
    
    public void InitializeHands(int playerCount)
    {
        allPlayerHands.Clear();
        
        // Create a hand for each player
        for (int i = 0; i < playerCount; i++)
        {
            allPlayerHands.Add(new PlayerHand(i));
        }
        
        // Deal 4 cards to each player
        DealInitialCards();
    }
    
    private void DealInitialCards()
    {
        foreach (var hand in allPlayerHands)
        {
            for (int i = 0; i < 4; i++)
            {
                Card card = CardDeck.Instance.DrawCard();
                if (card != null)
                {
                    hand.AddCard(card);
                }
            }
        }
        
        UpdateDisplay();
    }
    
    private void OnTurnChanged(int newPlayerIndex)
    {
        UpdateDisplay();
    }
    
    public void UpdateDisplay()
    {
        UpdateGoalCardDisplay();
        UpdateHandDisplay();
        UpdateDeckCount();
    }
    
    private void UpdateGoalCardDisplay()
    {
        if (CardDeck.Instance.currentGoal != null)
        {
            goalCardText.text = CardDeck.Instance.currentGoal.goalName;
            goalDescriptionText.text = CardDeck.Instance.currentGoal.description;
        }
    }
    
    private void UpdateHandDisplay()
    {
        // Clear existing cards
        foreach (var cardObj in currentCardUI)
        {
            Destroy(cardObj);
        }
        currentCardUI.Clear();
        
        // Get current player's hand
        int currentPlayer = TurnManager.Instance.GetCurrentPlayerIndex();
        if (currentPlayer >= allPlayerHands.Count) return;
        
        PlayerHand hand = allPlayerHands[currentPlayer];
        
        // Create card UI for each card in hand
        for (int i = 0; i < hand.cards.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            CardUI cardUI = cardObj.GetComponent<CardUI>();
            
            if (cardUI != null)
            {
                cardUI.SetCard(hand.cards[i], i);
                cardUI.OnCardClicked += OnCardSelected;
            }
            
            currentCardUI.Add(cardObj);
        }
        
        selectedCardIndex = -1;
    }
    
    private void UpdateDeckCount()
    {
        deckCountText.text = $"Deck: {CardDeck.Instance.GetDeckCount()}";
    }
    
    private void OnCardSelected(int index)
    {
        selectedCardIndex = index;
        Debug.Log($"Selected card at index {index}");
        
        // Visual feedback - highlight selected card
        for (int i = 0; i < currentCardUI.Count; i++)
        {
            CardUI cardUI = currentCardUI[i].GetComponent<CardUI>();
            if (cardUI != null)
            {
                cardUI.SetHighlight(i == selectedCardIndex);
            }
        }
    }
    
    private void OnPassCard()
    {
        if (selectedCardIndex < 0)
        {
            Debug.Log("No card selected to pass!");
            return;
        }
        
        int currentPlayer = TurnManager.Instance.GetCurrentPlayerIndex();
        PlayerHand currentHand = allPlayerHands[currentPlayer];
        
        // Get the card to pass
        Card cardToPass = currentHand.cards[selectedCardIndex];
        
        // Remove from current hand
        currentHand.RemoveCardAt(selectedCardIndex);
        
        // Add to next player's hand
        int nextPlayerIndex = (currentPlayer + 1) % allPlayerHands.Count;
        allPlayerHands[nextPlayerIndex].AddCard(cardToPass);
        
        Debug.Log($"Passed {cardToPass.number} {cardToPass.GetColorName()} to Player {nextPlayerIndex}");
        
        UpdateDisplay();
    }
    
    private void OnDrawCard()
    {
        int currentPlayer = TurnManager.Instance.GetCurrentPlayerIndex();
        PlayerHand currentHand = allPlayerHands[currentPlayer];
        
        if (currentHand.IsFull())
        {
            Debug.Log("Hand is full! Can't draw.");
            return;
        }
        
        Card drawnCard = CardDeck.Instance.DrawCard();
        if (drawnCard != null)
        {
            currentHand.AddCard(drawnCard);
            Debug.Log($"Drew {drawnCard.number} {drawnCard.GetColorName()}");
        }
        
        UpdateDisplay();
    }
    
    private void OnCheckMatch()
    {
        int currentPlayer = TurnManager.Instance.GetCurrentPlayerIndex();
        PlayerHand currentHand = allPlayerHands[currentPlayer];
        
        bool hasMatch = currentHand.CheckMatch(CardDeck.Instance.currentGoal);
        
        if (hasMatch)
        {
            Debug.Log("MATCH! Player wins this round!");
            // TODO: Trigger dice roll phase
            OnMatchFound();
        }
        else
        {
            Debug.Log("No match yet...");
        }
    }
    
   private void OnMatchFound()
{
    // Notify GameFlowManager to start dice phase
    if (GameFlowManager.Instance != null)
    {
        GameFlowManager.Instance.OnCardMatchFound();
    }
    else
    {
        Debug.LogError("GameFlowManager not found!");
    }
}
    
    public void ShowCardPhase()
    {
        cardHandPanel.SetActive(true);
        UpdateDisplay();
    }
    
    public void HideCardPhase()
    {
        cardHandPanel.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.OnTurnChanged -= OnTurnChanged;
        }
    }
}