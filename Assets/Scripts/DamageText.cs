using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshPro damageText;

    // Update is called once per frame
    public void UpdateText(string message)
    {
        transform.LookAt(FindObjectOfType<Camera>().transform);
        damageText.text = message;
    }
}
