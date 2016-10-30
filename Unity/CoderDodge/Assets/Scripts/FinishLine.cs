using UnityEngine;
using System.Collections.Generic;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameController.Instance.FinishGame();
    }
}
