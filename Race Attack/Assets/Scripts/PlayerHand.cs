using System.Collections.Generic;

public class PlayerHand
{
    public int playerIndex;
    public List<Card> cards = new List<Card>();
    public int maxHandSize = 4;
    
    public PlayerHand(int index)
    {
        playerIndex = index;
    }
    
    public void AddCard(Card card)
    {
        if (cards.Count < maxHandSize)
        {
            cards.Add(card);
        }
    }
    
    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }
    
    public void RemoveCardAt(int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            cards.RemoveAt(index);
        }
    }
    
    public void Clear()
    {
        cards.Clear();
    }
    
    public bool IsFull()
    {
        return cards.Count >= maxHandSize;
    }
    
    public bool CheckMatch(GoalCard goal)
    {
        return goal.ValidateHand(cards);
    }
}