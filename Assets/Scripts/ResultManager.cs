using UnityEngine;
using TMPro; // TextMeshPro kullanýyorsan
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("Text Alanlarý")]
    public TextMeshProUGUI kazancText;
    public TextMeshProUGUI toplamSiparisText;
    public TextMeshProUGUI iptalText;

    void Start()
    {
        // Oyun verilerini ekrana yazdýrýyoruz
        kazancText.text = GameData.GununKazanci.ToString() + " TL";
        toplamSiparisText.text = GameData.ToplamSiparis.ToString();
        iptalText.text = GameData.IptalEdilenSiparis.ToString();
    }

    // --- BUTON FONKSÝYONLARI ---

    public void YenidenBaslat()
    {
        // Verileri sýfýrla ve oyun sahnesine dön
        GameData.GununKazanci = 0;
        GameData.ToplamSiparis = 0;
        GameData.IptalEdilenSiparis = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void AnaMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Cikis()
    {
        Debug.Log("Oyundan çýkýlýyor...");
        Application.Quit();
    }
}