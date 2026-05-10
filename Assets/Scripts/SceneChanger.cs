using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Coroutine için bu satýr þart!

public class SceneChanger : MonoBehaviour
{
    // Butona týklandýðýnda bu fonksiyonu çaðýr
    public void StartGameWithDelay()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        Debug.Log("2 saniye sonra oyun baþlýyor...");

        // 2 saniye boyunca burada bekler
        yield return new WaitForSeconds(2f);

        // Bekleme süresi dolduðunda sahneyi yükler
        SceneManager.LoadScene("SampleScene");
    }
}