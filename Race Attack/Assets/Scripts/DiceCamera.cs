using UnityEngine;

public class DiceFollowCamera : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 5, -5);
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    
    [Header("Current Target")]
    public Transform currentDiceTarget;
    
    // Store original camera position and rotation
    private Vector3 homePosition;
    private Quaternion homeRotation;
    private Camera cam;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
        
        // Save the starting camera position/rotation
        homePosition = transform.position;
        homeRotation = transform.rotation;
        
        Dice.OnDiceResult += OnDiceFinished;
    }
    
    private void LateUpdate()
    {
        if (currentDiceTarget != null)
        {
            // Follow the dice
            Vector3 desiredPosition = currentDiceTarget.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            
            Quaternion targetRotation = Quaternion.LookRotation(currentDiceTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Return to home position when no target
            transform.position = Vector3.Lerp(transform.position, homePosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, homeRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        currentDiceTarget = newTarget;
        
        if (newTarget != null)
        {
            Debug.Log($"Camera now following: {newTarget.name}");
        }
        else
        {
            Debug.Log("Camera returning to home position");
        }
    }
    
    public void ReturnHome()
    {
        currentDiceTarget = null;
        Debug.Log("Camera returning home");
    }
    
    private void OnDiceFinished(int diceIndex, int result)
    {
        Debug.Log($"Dice {diceIndex} finished rolling: {result}");
    }
    
    private void OnDestroy()
    {
        Dice.OnDiceResult -= OnDiceFinished;
    }
}