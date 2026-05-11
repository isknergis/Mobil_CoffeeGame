using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public string selectedCoffee;
    public int sugarLevel = 0;
    public Text sugarText;

    void Start()
    {
        UpdateSugarUI();
    }

    public void SelectCoffee(string coffee, GameObject coffeeLight)
    {
        // 1. Önce sahnede yanan TÜM kahve buton ýþýklarýný bul ve söndür
        // (Sadece bir tane kahve seçilebildiði için temizlik þart)
        CoffeeButton[] allButtons = FindObjectsByType<CoffeeButton>(FindObjectsSortMode.None);
        foreach (CoffeeButton btn in allButtons)
        {
            if (btn.selectionLight != null) btn.selectionLight.SetActive(false);
        }

        // 2. Seçilen kahvenin ismini kaydet ve ýþýðýný yak
        selectedCoffee = coffee;
        if (coffeeLight != null) coffeeLight.SetActive(true);

        Debug.Log("Seçilen kahve: " + coffee);
    }

    public void NextSugar()
    {
        sugarLevel = (sugarLevel + 1) % 3;
        UpdateSugarUI();
    }

    void UpdateSugarUI()
    {
        string text = sugarLevel == 0 ? "Sade" :
                      sugarLevel == 1 ? "Orta" : "Çok";

        if (sugarText != null)
            sugarText.text = "Þeker: " + text;
    }

    public void ResetSelection()
    {
        selectedCoffee = null;
        sugarLevel = 0;
        UpdateSugarUI();

        // RESET: Tüm ýþýklarý söndür
        CoffeeButton[] allButtons = FindObjectsByType<CoffeeButton>(FindObjectsSortMode.None);
        foreach (CoffeeButton btn in allButtons)
        {
            if (btn.selectionLight != null) btn.selectionLight.SetActive(false);
        }
    }
}