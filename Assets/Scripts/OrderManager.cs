using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [Header("Order")]
    public List<string> coffeeTypes;
    public List<string> unlockedAromas;

    public int playerLevel = 1;
    public CoffeeOrder currentOrder;

    [Header("UI")]
    public Text orderText;
    public Image patienceBar;

    [Header("Patience Settings")]
    public float basePatience = 60f; // Baþlangýç sabýr süresi (Örn: 60 saniye)
    public float minPatience = 15f;  // Sabrýn düþebileceði en alt sýnýr
    float currentTime;
    float currentMaxPatience; // O anki seviye için hesaplanan maksimum sabýr

    GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (currentOrder == null) GenerateOrder();
    }

    void Update()
    {
        if (currentOrder == null) return;

        currentTime -= Time.deltaTime;

        if (patienceBar != null)
            // Oranlarken o seviyeye özel belirlenen süreyi kullanýyoruz
            patienceBar.fillAmount = currentTime / currentMaxPatience;

        if (currentTime <= 0)
        {
            Debug.Log("Süre doldu!");
            if (gameManager != null)
                gameManager.OnOrderFailed();

            GenerateOrder();
        }
    }

    public void GenerateOrder()
    {
        currentOrder = new CoffeeOrder();

        // 1. SEVÝYE BÝLGÝSÝNÝ GÜNCELLE
        // GameManager'dan güncel seviyeyi çekiyoruz
        if (gameManager != null)
        {
            playerLevel = gameManager.orderManager.playerLevel;
            unlockedAromas = gameManager.unlockSystem.unlockedAromas;
        }

        // 2. SABIR SÜRESÝNÝ SEVÝYEYE GÖRE HESAPLA
        // Her seviye için sabýr süresini 5 saniye azaltýyoruz
        // Formül: Baþlangýç Sabrý - ((Seviye - 1) * 5)
        float calculatedPatience = basePatience - ((playerLevel - 1) * 5f);

        // Sabrýn çok aþýrý düþüp oyunu imkansýz yapmasýný engellemek için sýnýrlýyoruz
        currentMaxPatience = Mathf.Max(minPatience, calculatedPatience);
        currentTime = currentMaxPatience;

        // --- Geri kalan sipariþ oluþturma kodlarýn ---
        if (coffeeTypes != null && coffeeTypes.Count > 0)
        {
            currentOrder.coffeeType = coffeeTypes[Random.Range(0, coffeeTypes.Count)];
        }

        currentOrder.sugarLevel = Random.Range(0, 3);
        currentOrder.aromas = new List<string>();

        if (unlockedAromas != null && unlockedAromas.Count > 0)
        {
            int maxAroma = Mathf.Min(2, unlockedAromas.Count);
            int aromaCount = Random.Range(0, maxAroma + 1);

            for (int i = 0; i < aromaCount; i++)
            {
                string a = unlockedAromas[Random.Range(0, unlockedAromas.Count)];
                if (!currentOrder.aromas.Contains(a))
                    currentOrder.aromas.Add(a);
            }
        }

        UpdateUI();
        Debug.Log("<color=red>Müþteri Sabrý: </color>" + currentMaxPatience + " saniye (Seviye: " + playerLevel + ")");
    }

    void UpdateUI()
    {
        if (orderText == null) return;

        string sugarText = currentOrder.sugarLevel == 0 ? "Sade" :
                           currentOrder.sugarLevel == 1 ? "Orta" : "Çok";

        string aromaText = (currentOrder.aromas != null && currentOrder.aromas.Count > 0)
                           ? string.Join(", ", currentOrder.aromas) : "Yok";

        orderText.text = currentOrder.coffeeType + "\n" +
                        "Þeker: " + sugarText + "\n" +
                        "Aroma: " + aromaText;
    }
}