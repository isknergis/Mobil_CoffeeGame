using UnityEngine;
using UnityEngine.UI;

public class AromaButton : MonoBehaviour
{
    public string aromaName;
    public Image buttonImage;
    public GameObject selectionLight;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;
    public Color lockedColor = Color.gray; // Kilitli renk eklendi

    private AromaSelection aromaSelection;
    private GameManager gameManager;
    private bool isSelected = false;
    private bool isUnlocked = false; // Uyarýyý bu deðiþkeni kullanarak gidereceðiz

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
            // Deðiþkeni burada kullanýyoruz (Atama yapýlýyor)
            isUnlocked = gameManager.unlockSystem.unlockedAromas.Contains(aromaName);
        }

        // Deðiþkeni burada kontrol ediyoruz (Kullaným yapýlýyor -> Uyarý giderilir)
        if (!isUnlocked)
        {
            if (buttonImage != null) buttonImage.color = lockedColor;
            if (selectionLight != null) selectionLight.SetActive(false);
        }
        else
        {
            UpdateVisual(); // Kilit açýksa görseli güncelle
        }
    }

    // AromaButton.cs içindeki OnClick fonksiyonunu bu þekilde güncelle
    public void OnClick()
    {
        if (!isUnlocked || (gameManager != null && !gameManager.CanSelect())) return;

        isSelected = !isSelected;

        // Iþýðý ve rengi doðrudan isSelected durumuna baðla (Gecikme olmaz)
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
        // KULLANIM: Sadece kilit açýksa görsel deðiþim yap
        if (!isUnlocked) return;

        if (buttonImage != null)
            buttonImage.color = isSelected ? selectedColor : normalColor;

        if (selectionLight != null)
            selectionLight.SetActive(isSelected);
    }

    public void ResetButton()
    {
        isSelected = false;
        CheckUnlockStatus(); // Reset atarken kilit durumunu da tazeler
    }
}