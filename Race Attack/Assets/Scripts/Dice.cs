using UnityEngine;
using UnityEngine.Events;
using System.Collections; // Changed from System.Threading.Tasks

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    public Transform[] diceFaces;
    public Rigidbody rb;

    private int dice_Index = -1;

    private bool hasStoppedRolling;
    private bool delay_Finished;

    public static UnityAction<int, int> OnDiceResult;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!delay_Finished) return;

        if (!hasStoppedRolling && rb.linearVelocity.sqrMagnitude == 0f)
        {
            hasStoppedRolling = true;
            GetNumberOnTopFace();
        }
    }

    public void RollDice(float throwForce, float rollForce, int i)
    {
        dice_Index = i;
        hasStoppedRolling = false;
        delay_Finished = false;

        var randomVariance = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce + randomVariance), ForceMode.Impulse);

        var randX = Random.Range(0f, 1f);
        var randY = Random.Range(0f, 1f);
        var randZ = Random.Range(0f, 1f);

        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollForce + randomVariance), ForceMode.Impulse);

        StartCoroutine(DelayResult()); // Changed to Coroutine
    }

    // Changed from async to Coroutine
    private IEnumerator DelayResult()
    {
        yield return new WaitForSeconds(1f); // Wait 1 second
        delay_Finished = true;
    }

    [ContextMenu(itemName: "Get Top Face")]
    private int GetNumberOnTopFace()
    {
        if (diceFaces == null || diceFaces.Length == 0)
        {
            Debug.LogWarning("Dice faces not assigned!");
            return -1;
        }

        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;

        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }
        Debug.Log($"Dice result {topFace + 1}");

        OnDiceResult?.Invoke(dice_Index, topFace + 1);

        return topFace + 1;
    }
}