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
        // Baþlangýç kilitlerini aç
        unlockSystem.unlockedAromas = new List<string> { "Vanilya", "Kakao" };
        orderManager.unlockedAromas = unlockSystem.unlockedAromas;

        ResetAllSystem(); // Her þeyi tertemiz baþlat
        orderManager.GenerateOrder();

        serveButton.interactable = false;
        if (machine != null) machine.OnCoffeeReady += EnableServeButton;

        UpdateMoneyUI();

        if (gunuBitirButton != null)
            gunuBitirButton.onClick.AddListener(GunuBitir);
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
            GameData.ToplamSiparis++;
            Debug.Log("DOÐRU +" + earned);
        }
        else
        {
            money -= 5;
            GameData.IptalEdilenSiparis++;
            Debug.Log("YANLIÞ");
        }

        // Temizlik ve yeni sipariþ
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

    // Hem servis bittiðinde hem müþteri kaçtýðýnda çalýþan ANA temizlik
    void ResetAllSystem()
    {
        // Önce butonlarý ve aromalarý sýfýrla
        ResetSelections();

        // Sonra makineyi temizle
        if (machine != null)
        {
            machine.ResetMachine();
        }

        serveButton.interactable = false;
    }
    public void ResetSelections()
    {
        player.ResetSelection();
        aroma.ResetAromas();
        foreach (var btn in aromaButtons)
        {
            if (btn != null) btn.ResetButton();
        }
    }

    bool CheckOrder()
    {
        var order = orderManager.currentOrder;

        // Kahve ve þeker kontrolü
        if (order.coffeeType != player.selectedCoffee) return false;
        if (order.sugarLevel != player.sugarLevel) return false;

        // --- AROMA KESÝN EÞLEÞME KONTROLÜ ---
        int orderCount = (order.aromas != null) ? order.aromas.Count : 0;
        int selectedCount = (aroma.selectedAromas != null) ? aroma.selectedAromas.Count : 0;

        // Sayýlar tutmuyorsa (fazla veya eksik aroma) direkt yanlýþ
        if (orderCount != selectedCount) return false;

        // Ýçerik kontrolü
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

    void UpdateMoneyUI() { if (moneyText != null) moneyText.text = "Para: " + money; }
    public void GunuBitir() { SceneManager.LoadScene("FinishScene"); }
}