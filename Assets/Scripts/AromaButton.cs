using UnityEngine;
using UnityEngine.UI;

public class AromaButton : MonoBehaviour
{
    public string aromaName;
    public Image buttonImage;

    [Header("Iþýklar")]
    public GameObject selectionLight; // Seçince yanan ýþýk (Yeþil vb.)
    public GameObject lockedLight;    // Kilitliyken yanan gri ýþýk

    [Header("Renkler")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;
    public Color lockedColor = Color.gray;

    private AromaSelection aromaSelection;
    private GameManager gameManager;
    private bool isSelected = false;
    private bool isUnlocked = false;

    void Start()
    {
        aromaSelection = FindFirstObjectByType<AromaSelection>();
        gameManager = FindFirstObjectByType<GameManager>();

        // Baþlangýçta kilit durumunu kontrol et
        CheckUnlockStatus();
    }

    public void CheckUnlockStatus()
    {
        if (gameManager != null && gameManager.unlockSystem != null)
        {
            isUnlocked = gameManager.unlockSystem.unlockedAromas.Contains(aromaName);
        }

        if (!isUnlocked)
        {
            // --- KÝLÝTLÝ DURUM ---
            if (buttonImage != null) buttonImage.color = lockedColor;

            if (selectionLight != null) selectionLight.SetActive(false); // Seçim ýþýðýný kapat
            if (lockedLight != null) lockedLight.SetActive(true);        // GRÝ KÝLÝT IÞIÐINI AÇ
        }
        else
        {
            // --- KÝLÝT AÇILDI DURUMU ---
            if (lockedLight != null) lockedLight.SetActive(false);       // GRÝ KÝLÝT IÞIÐINI SÖNDÜR

            if (buttonImage != null && !isSelected) buttonImage.color = normalColor;
        }
    }

    public void OnClick()
    {
        // Kilitliyse veya makine çalýþýyorsa týklamayý engelle
        if (!isUnlocked || (gameManager != null && !gameManager.CanSelect())) return;

        isSelected = !isSelected;

        // Iþýðý ve rengi duruma göre güncelle
        if (selectionLight != null) selectionLight.SetActive(isSelected);
        if (buttonImage != null) buttonImage.color = isSelected ? selectedColor : normalColor;

        if (aromaSelection != null)
        {
            if (isSelected) aromaSelection.AddAroma(aromaName);
            else aromaSelection.RemoveAroma(aromaName);
        }
    }

    public void UpdateVisual()
    {
        if (!isUnlocked) return;

        if (buttonImage != null)
            buttonImage.color = isSelected ? selectedColor : normalColor;

        if (selectionLight != null)
            selectionLight.SetActive(isSelected);
    }

    public void ResetButton()
    {
        isSelected = false;

        // Resetlendiðinde seçim ýþýðýný kapat, kilit ýþýðýný CheckUnlockStatus'a býrak
        if (selectionLight != null) selectionLight.SetActive(false);

        CheckUnlockStatus();
    }
}