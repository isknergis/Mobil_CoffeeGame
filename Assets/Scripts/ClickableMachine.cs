using UnityEngine;

public class ClickableMachine : MonoBehaviour
{
    public CoffeeMachine machine;

    void OnMouseDown()
    {
        machine.StartBrewing();
    }
}