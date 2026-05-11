using UnityEngine;

public class CoffeeButton : MonoBehaviour
{
    public string coffeeName;
    public GameObject selectionLight; // Butonun Point Light'ý

    private PlayerSelection playerSelection;

    void Start()
    {
        playerSelection = FindFirstObjectByType<PlayerSelection>();
        if (selectionLight != null) selectionLight.SetActive(false);
    }

    public void OnClick()
    {
        if (playerSelection != null)
        {
            playerSelection.SelectCoffee(coffeeName, selectionLight);
        }
    }
}