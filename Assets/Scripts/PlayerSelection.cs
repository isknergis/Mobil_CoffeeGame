using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    public string selectedCoffee;
    public int sugarLevel = 0;
    public Text sugarText;

    [Header("Þeker Görselleþtirmesi")]
    public Light sugarPointLight; // Bardaðýn içindeki Point Light
    public float baseIntensity = 10f;

    void Start()
    {
        UpdateSugarUI();
    }

    public void SelectCoffee(string coffee, GameObject coffeeLight)
    {
        // 1. SADECE Kahve Butonlarýnýn üzerindeki seçim ýþýklarýný kapatýyoruz
        CoffeeButton[] allButtons = FindObjectsByType<CoffeeButton>(FindObjectsSortMode.None);
        foreach (CoffeeButton btn in allButtons)
        {
            // Þeker ýþýðýný buradan ayýrdýk, artýk o yanlýþlýkla sönmeyecek
            if (btn.selectionLight != null)
                btn.selectionLight.SetActive(false);
        }

        // 2. Seçilen kahveyi kaydet ve buton ýþýðýný yak
        selectedCoffee = coffee;
        if (coffeeLight != null) coffeeLight.SetActive(true);

        // ÖNEMLÝ: Kahve seçince þeker seviyesi sýfýrlanmasýn istiyorsan burayý böyle býrakýyoruz.
        // Ama kahve seçince þeker ýþýðý da güncellensin dersen þu satýrý ekleyebilirsin:
        UpdateSugarUI();

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

        // ÞEKER IÞIÐI KONTROLÜ
        if (sugarPointLight != null)
        {
            if (sugarLevel == 0)
            {
                sugarPointLight.intensity = 0; // Kapalý
            }
            else if (sugarLevel == 1)
            {
                sugarPointLight.intensity = baseIntensity; // Orta
                sugarPointLight.color = new Color(1f, 0.9f, 0.7f); // Sýcak bir ton
            }
            else
            {
                sugarPointLight.intensity = baseIntensity * 3f; // Çok Parlak
                sugarPointLight.color = Color.white;
            }
        }
    }

    public void ResetSelection()
    {
        selectedCoffee = null;
        sugarLevel = 0;
        UpdateSugarUI(); // Iþýk burada 0 olacak

        // Buton ýþýklarýný temizle
        CoffeeButton[] allButtons = FindObjectsByType<CoffeeButton>(FindObjectsSortMode.None);
        foreach (CoffeeButton btn in allButtons)
        {
            if (btn.selectionLight != null) btn.selectionLight.SetActive(false);
        }
    }
}