using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public string selectedCoffee;
    public int sugarLevel = 0; // 0=Sade, 1=Orta, 2=Çok

    public Text sugarText;

    void Start()
    {
        UpdateSugarUI();
    }

    public void SelectCoffee(string coffee)
    {
        selectedCoffee = coffee;
        Debug.Log("Seçilen kahve: " + coffee);
    }

    // TEK BUTON ? þeker döngüsü
    public void NextSugar()
    {
        sugarLevel = (sugarLevel + 1) % 3;
        UpdateSugarUI();
    }

    void UpdateSugarUI()
    {
        string text = sugarLevel == 0 ? "Sade" :
                      sugarLevel == 1 ? "Orta" : "Çok";

        Debug.Log("Þeker: " + text);

        if (sugarText != null)
            sugarText.text = "Þeker: " + text;
    }

    public void ResetSelection()
    {
        selectedCoffee = null;
        sugarLevel = 0;
        UpdateSugarUI();
    }
}