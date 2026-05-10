using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Card
{
    public int number; // 1-10
    public CardColor color;
    
    public enum CardColor
    {
        Red,
        Blue,
        Yellow,
        Green
    }
    
    public Card(int num, CardColor col)
    {
        number = num;
        color = col;
    }
    
    public Color GetUnityColor()
    {
        switch (color)
        {
            case CardColor.Red: return Color.red;
            case CardColor.Blue: return Color.blue;
            case CardColor.Yellow: return Color.yellow;
            case CardColor.Green: return Color.green;
            default: return Color.white;
        }
    }
    
    public string GetColorName()
    {
        return color.ToString();
    }
}

public class GoalCard
{
    public string goalName;
    public string description;
    public GoalType type;
    
    public enum GoalType
    {
        AllSameColor,
        AllDifferentColors,
    }
    
    public GoalCard(GoalType goalType)
    {
        type = goalType;
        
        switch (goalType)
        {
            case GoalType.AllSameColor:
                goalName = "All Same Color";
                description = "Get 3 cards of the same color";
                break;
            case GoalType.AllDifferentColors:
                goalName = "Rainbow";
                description = "Get all 4 different colors";
                break;
        }
    }
    
    public bool ValidateHand(List<Card> hand)
    {
        if (hand.Count < 3) return false;
        
        switch (type)
        {
            case GoalType.AllSameColor:
                return ValidateAllSameColor(hand);
            case GoalType.AllDifferentColors:
                return ValidateAllDifferentColors(hand);
            default:
                return false;
        }
    }
    
    
    private bool ValidateAllSameColor(List<Card> hand)
    {
        if (hand.Count < 3) return false;
        var firstColor = hand[0].color;
        return hand.Take(3).All(c => c.color == firstColor);
    }
    
   /* private bool ValidateSequential(List<Card> hand)
    {
        var sorted = hand.OrderBy(c => c.number).ToList();
        
        for (int i = 0; i <= sorted.Count - 3; i++)
        {
            if (sorted[i].number + 1 == sorted[i + 1].number &&
                sorted[i + 1].number + 1 == sorted[i + 2].number)
            {
                return true;
            }
        }
        return false;
    }
    */
    
    private bool ValidateAllDifferentColors(List<Card> hand)
    {
        if (hand.Count < 4) return false;
        var colors = hand.Select(c => c.color).Distinct();
        return colors.Count() >= 4;
    }
    
    /*private bool ValidateSumTo15(List<Card> hand)
    {
        // Check all combinations of 3 cards
        for (int i = 0; i < hand.Count - 2; i++)
        {
            for (int j = i + 1; j < hand.Count - 1; j++)
            {
                for (int k = j + 1; k < hand.Count; k++)
                {
                    if (hand[i].number + hand[j].number + hand[k].number == 15)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    } */
}

public class CardDeck : MonoBehaviour
{
    public static CardDeck Instance { get; private set; }
    
    private List<Card> deck = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    public GoalCard currentGoal { get; private set; }
    
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
    
    public void InitializeDeck()
{
    deck.Clear();
    discardPile.Clear();
    
    // Create 80 cards: TWO of each number 1-10 in 4 colors
    for (int copies = 0; copies < 4; copies++) // Add 2 copies of the deck
    {
        foreach (Card.CardColor color in System.Enum.GetValues(typeof(Card.CardColor)))
        {
            for (int num = 1; num <= 10; num++)
            {
                deck.Add(new Card(num, color));
            }
        }
    }
    
    ShuffleDeck();
    DrawNewGoalCard();
}
    public void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        deck = deck.OrderBy(c => rng.Next()).ToList();
    }
    
    public Card DrawCard()
    {
        if (deck.Count == 0)
        {
            // Reshuffle discard pile into deck
            deck = new List<Card>(discardPile);
            discardPile.Clear();
            ShuffleDeck();
        }
        
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            return drawnCard;
        }
        
        return null;
    }
    
    public void DiscardCard(Card card)
    {
        discardPile.Add(card);
    }
    
    public void DrawNewGoalCard()
    {
        // Randomly select a goal type
        System.Array goalTypes = System.Enum.GetValues(typeof(GoalCard.GoalType));
        GoalCard.GoalType randomType = (GoalCard.GoalType)goalTypes.GetValue(Random.Range(0, goalTypes.Length));
        currentGoal = new GoalCard(randomType);
        
        Debug.Log($"New Goal: {currentGoal.goalName}");
    }
    
    public int GetDeckCount()
    {
        return deck.Count;
    }
}
