using UnityEngine;
using System.Collections;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject cupPrefab;
    public Transform spawnPoint;
    public Light machineLight;
    public float brewTime = 3f;

    private bool isBrewing = false;
    private bool coffeeReady = false;

    public System.Action OnCoffeeReady;

    // --- ARTIK POZÝSYON KAYDETMÝYORUZ, OLDUÐU GÝBÝ BIRAKIYORUZ ---

    public void StartBrewing()
    {
        if (isBrewing || coffeeReady) return;
        StartCoroutine(BrewProgress());
    }

    IEnumerator BrewProgress()
    {
        isBrewing = true;

        // Iþýðý aç
        if (machineLight != null) machineLight.enabled = true;

        // BEKLEME SÜRESÝ (Titreme yok, sadece bekliyor)
        yield return new WaitForSeconds(brewTime);

        // Iþýðý kapat
        if (machineLight != null) machineLight.enabled = false;

        // FÝNCAN OLUÞTUR
        if (cupPrefab != null && spawnPoint != null)
        {
            // Fincaný oluþturur, makinenin yerini asla deðiþtirmez
            Instantiate(cupPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        isBrewing = false;
        coffeeReady = true;
        OnCoffeeReady?.Invoke();
    }

    public void ResetMachine()
    {
        // Resetlendiðinde sadece durumlarý sýfýrla
        StopAllCoroutines();
        isBrewing = false;
        coffeeReady = false;

        if (machineLight != null) machineLight.enabled = false;

        // SADECE FÝNCANLARI SÝL
        GameObject[] cups = GameObject.FindGameObjectsWithTag("Fincan");
        foreach (GameObject cup in cups)
        {
            if (cup != this.gameObject)
            {
                Destroy(cup);
            }
        }
    }

    public bool IsBrewing() => isBrewing;
    public bool IsCoffeeReady() => coffeeReady;
}