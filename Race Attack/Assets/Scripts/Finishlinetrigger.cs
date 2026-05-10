using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if a car crossed the finish line
        if (other.CompareTag("Player") || other.CompareTag("Boss"))
        {
            string winnerName = other.gameObject.name;
            
            Debug.Log($"{winnerName} crossed the finish line! GAME OVER!");
            
            // Trigger game over
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.TriggerGameOver(winnerName);
            }
        }
    }
}