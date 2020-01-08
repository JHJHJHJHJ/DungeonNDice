using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionMessage : MonoBehaviour
{
    TextMeshProUGUI instructionMessage;

    private void Awake() 
    {
        instructionMessage = GetComponent<TextMeshProUGUI>();    
    }

    private void Start() 
    {
        UpdateMessage("");    
    }

    public void UpdateMessage(string messageToUpdate)
    {
        instructionMessage.text = messageToUpdate;
    }
}
