using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardUI : MonoBehaviour
{
    public Image cardBackground;
    public TextMeshProUGUI numberText;
    public TextMeshProUGUI colorText;
    public Button cardButton;
    public Image highlightBorder;
    
    private Card card;
    private int cardIndex;
    
    public event Action<int> OnCardClicked;
    
    private void Awake()
    {
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(OnClick);
        }
        
        if (highlightBorder != null)
        {
            highlightBorder.gameObject.SetActive(false);
        }
    }
    
    public void SetCard(Card newCard, int index)
    {
        card = newCard;
        cardIndex = index;
        
        if (numberText != null)
        {
            numberText.text = card.number.ToString();
        }
        
        if (colorText != null)
        {
            colorText.text = card.GetColorName();
        }
        
        if (cardBackground != null)
        {
            cardBackground.color = card.GetUnityColor();
        }
    }
    
    private void OnClick()
    {
        OnCardClicked?.Invoke(cardIndex);
    }
    
    public void SetHighlight(bool highlighted)
    {
        if (highlightBorder != null)
        {
            highlightBorder.gameObject.SetActive(highlighted);
        }
    }
}