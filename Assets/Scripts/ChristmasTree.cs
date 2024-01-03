using UnityEngine;
using TMPro;

public class ChristmasTreeInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f; // Adjust the distance as needed
    public TMP_Text messageText;

    private bool isInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the TMP text initially
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isInRange)
        {
            DisplayMessage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;

            // Display the message using TMPro
            if (messageText != null)
            {
                messageText.text = "You have delivered the presents, Merry Christmas!!";
                messageText.gameObject.SetActive(true);
            }
        }
    }

    private void DisplayMessage()
    {
        // Your interaction logic here
        Debug.Log("Performing automatic interaction with the Christmas tree");
        // You can add more actions or animations here
    }
}
