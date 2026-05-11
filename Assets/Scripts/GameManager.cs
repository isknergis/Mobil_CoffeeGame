using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public OrderManager orderManager;
    public PlayerSelection player;
    public AromaSelection aroma;
    public UnlockSystem unlockSystem;
    public CoffeeMachine machine;

    [Header("UI")]
    public Button serveButton;
    public Text moneyText;
    public Button gunuBitirButton;

    [Header("Buttons")]
    public List<AromaButton> aromaButtons;

    [Header("Economy")]
    public List<CoffeeData> coffeePrices;

    [System.Serializable]
    public class AromaData
    {
        public string aromaName;
        public int price;
    }

    public List<AromaData> aromaPrices;
    int money = 0;

    void Start()
    {
        // 1. BAÞLANGIÇ AYARLARI
        // Ýlk açýlýþta temel aromalar her zaman açýk olsun
        unlockSystem.unlockedAromas = new List<string> { "Vanilya", "Kakao" };

        // Mevcut sipariþ sayýsýna göre seviyeyi en baþta bir kez hesapla
        UpdateLevelLogic();

        ResetAllSystem();
        orderManager.GenerateOrder();

        serveButton.interactable = false;
        if (machine != null) machine.OnCoffeeReady += EnableServeButton;

        UpdateMoneyUI();

        if (gunuBitirButton != null)
            gunuBitirButton.onClick.AddListener(GunuBitir);
    }

    // --- SEVÝYE HESAPLAMA MANTIÐI ---
    public void UpdateLevelLogic()
    {
        // ÖRNEK: Her 3 doðru sipariþte 1 seviye atla (+1 baþlangýç seviyesi)
        // 0-2 sipariþ: Seviye 1 | 3-5 sipariþ: Seviye 2 | 6-8 sipariþ: Seviye 3...
        int newLevel = (GameData.ToplamSiparis / 3) + 1;

        // Seviyeyi OrderManager'a iþle
        orderManager.playerLevel = newLevel;

        // Kilit sistemini yeni seviyeye göre çalýþtýr
        unlockSystem.CheckUnlock(newLevel);

        // Sipariþ sistemine açýlan yeni aromalarý bildir
        orderManager.unlockedAromas = new List<string>(unlockSystem.unlockedAromas);

        Debug.Log("<color=yellow>Mevcut Seviye: </color>" + newLevel + " | Toplam Sipariþ: " + GameData.ToplamSiparis);
    }

    void EnableServeButton() { serveButton.interactable = true; }

    public bool CanSelect() { return !machine.IsBrewing(); }

    public void OnServeButton()
    {
        serveButton.interactable = false;
        bool correct = CheckOrder();

        if (correct)
        {
            int earned = CalculateEarnings(orderManager.currentOrder.coffeeType);
            money += earned;
            GameData.GununKazanci += earned;
            GameData.ToplamSiparis++; // Sipariþ sayýsý burada artýyor

            // DOÐRU SERVÝS SONRASI: Seviyeyi ve kilitleri kontrol et
            UpdateLevelLogic();

            Debug.Log("<color=green>DOÐRU +</color>" + earned);
        }
        else
        {
            money -= 5;
            GameData.IptalEdilenSiparis++;
            Debug.Log("<color=red>YANLIÞ</color>");
        }

        ResetAllSystem();
        orderManager.GenerateOrder();
        UpdateMoneyUI();
    }

    public void OnOrderFailed()
    {
        money -= 10;
        ResetAllSystem();
        Debug.Log("Müþteri kaçtý! -10");
        UpdateMoneyUI();
    }

    void ResetAllSystem()
    {
        ResetSelections();
        if (machine != null) machine.ResetMachine();
        serveButton.interactable = false;
    }

    public void ResetSelections()
    {
        player.ResetSelection();
        aroma.ResetAromas();
        // Butonlarýn görselini ve kilit durumunu tazele
        foreach (var btn in aromaButtons)
        {
            if (btn != null) btn.ResetButton();
        }
    }

    bool CheckOrder()
    {
        var order = orderManager.currentOrder;
        if (order.coffeeType != player.selectedCoffee) return false;
        if (order.sugarLevel != player.sugarLevel) return false;

        int orderCount = (order.aromas != null) ? order.aromas.Count : 0;
        int selectedCount = (aroma.selectedAromas != null) ? aroma.selectedAromas.Count : 0;

        if (orderCount != selectedCount) return false;

        if (orderCount > 0)
        {
            foreach (var a in order.aromas)
            {
                if (!aroma.selectedAromas.Contains(a)) return false;
            }
        }

        if (!machine.IsCoffeeReady()) return false;

        return true;
    }

    int GetCoffeePrice(string coffeeName)
    {
        foreach (var coffee in coffeePrices)
            if (coffee.coffeeName == coffeeName) return coffee.basePrice;
        return 10;
    }

    int GetAromaPrice(string aromaName)
    {
        foreach (var a in aromaPrices)
            if (a.aromaName == aromaName) return a.price;
        return 0;
    }

    int CalculateEarnings(string coffeeName)
    {
        int total = GetCoffeePrice(coffeeName);
        if (orderManager.currentOrder.aromas != null)
            foreach (var a in orderManager.currentOrder.aromas) total += GetAromaPrice(a);

        float multiplier = 1f + (orderManager.playerLevel * 0.05f);
        return Mathf.RoundToInt(total * multiplier);
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Para: " + GameData.GununKazanci;
    }
    public void GunuBitir() { SceneManager.LoadScene("FinishScene"); }
}