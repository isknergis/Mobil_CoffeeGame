using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour
{
    // Inspector'da listenin dolduðunu oyun içinde buradan takip edebilirsin
    public List<string> unlockedAromas = new List<string>();
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void CheckUnlock(int level)
    {
        bool change = false;

        // Seviye Kontrolleri
        if (level >= 2)
        {
            if (AddAroma("Fýndýk")) change = true;
            if (AddAroma("Karamel")) change = true;
        }
        if (level >= 3)
        {
            if (AddAroma("Fýstýk")) change = true;
            if (AddAroma("Bal")) change = true;
        }
        if (level >= 4)
        {
            if (AddAroma("Tarçýn")) change = true;
            if (AddAroma("Damla Sakýzý")) change = true;
        }

        if (change)
        {
            SyncEverything();
        }
    }

    private bool AddAroma(string aromaName)
    {
        if (!unlockedAromas.Contains(aromaName))
        {
            unlockedAromas.Add(aromaName);
            return true;
        }
        return false;
    }

    public void SyncEverything()
    {
        // 1. Butonlarý Güncelle (Gri ýþýklar sönsün, kilitler kalksýn)
        AromaButton[] buttons = FindObjectsByType<AromaButton>(FindObjectsSortMode.None);
        foreach (var btn in buttons) btn.CheckUnlockStatus();

        // 2. OrderManager'ý Güncelle (Yeni kahveler sipariþ olarak gelebilsin)
        if (gameManager != null && gameManager.orderManager != null)
        {
            // Sipariþ yöneticisine "artýk bunlarý da isteyebilirsin" diyoruz
            gameManager.orderManager.unlockedAromas = new List<string>(unlockedAromas);
            Debug.Log("<color=cyan>Sipariþ listesi güncellendi!</color>");
        }
    }
}