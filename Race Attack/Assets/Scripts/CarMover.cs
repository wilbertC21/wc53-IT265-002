using UnityEngine;

public class CarMover : MonoBehaviour
{
    public void MoveForward(int diceRoll)
    {
        // Move forward along X axis by dice roll * 2
        float moveAmount = diceRoll * 90f;
        transform.position += new Vector3(moveAmount, 0, 0);
        
        Debug.Log($"{gameObject.name} moved forward {moveAmount} units on X axis (dice roll: {diceRoll})");
    }
    
    public void MoveBackward(int diceRoll)
    {
        // Move backward along X axis by dice roll * 2
        float moveAmount = diceRoll * 2f;
        transform.position -= new Vector3(moveAmount, 0, 0);
        
        Debug.Log($"{gameObject.name} moved backward {moveAmount} units on X axis (dice roll: {diceRoll})");
    }
}