using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DiceThrow : MonoBehaviour
{
    public static DiceThrow Instance { get; private set; }
    
    public Dice diceToThrow;
    public int amountOfDice = 3;
    public float throwForce = 5f;
    public float rollForce = 10f;
    
    [Header("Camera")]
    public DiceFollowCamera diceCamera;

    private List<GameObject> spawnedDice = new List<GameObject>();
    private int currentDiceIndex = 0;

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
        ResetDice();
    }

    private void Update()
    {
        // Manual dice rolling with Space (only if not controlled by GameFlowManager)
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetDice();
        }
    }

    public void RollSingleDice()
    {
        if (diceToThrow == null)
        {
            Debug.LogWarning("Dice prefab is not assigned!");
            return;
        }

        if (currentDiceIndex >= amountOfDice)
        {
            Debug.Log("All dice thrown! Resetting...");
            ResetDice();
            return;
        }

        Vector3 spawnPos = transform.position + new Vector3(currentDiceIndex * 2, 1, 0);
        Dice dice = Instantiate(diceToThrow, spawnPos, transform.rotation);
        
        if (dice != null)
        {
            spawnedDice.Add(dice.gameObject);
            dice.RollDice(throwForce, rollForce, currentDiceIndex);
            currentDiceIndex++;
            
            // Make camera follow this dice
            if (diceCamera != null)
            {
                diceCamera.SetTarget(dice.transform);
            }
            
            Debug.Log($"Threw dice {currentDiceIndex}/{amountOfDice}");
        }
    }

    public void ResetDice()
    {
        GameObject[] diceCopy = spawnedDice.ToArray();
        
        foreach (var die in diceCopy)
        {
            if (die != null)
            {
                Destroy(die);
            }
        }
        
        spawnedDice.Clear();
        currentDiceIndex = 0;
        
        // Reset camera target
        if (diceCamera != null)
        {
            diceCamera.SetTarget(null);
        }
        
        Debug.Log("Dice reset!");
    }

    private void OnDestroy()
    {
        ResetDice();
    }
}