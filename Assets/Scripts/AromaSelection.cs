using System.Collections.Generic;
using UnityEngine;

public class AromaSelection : MonoBehaviour
{
    public List<string> selectedAromas = new List<string>();
    public UnlockSystem unlockSystem;
    public void AddAroma(string aroma)
    {
        if (!selectedAromas.Contains(aroma)) selectedAromas.Add(aroma);
    }

    public void RemoveAroma(string aroma)
    {
        if (selectedAromas.Contains(aroma)) selectedAromas.Remove(aroma);
    }
  

    public void ResetAromas()
    {
        selectedAromas.Clear();
        Debug.Log("Aroma listesi tamamen temizlendi.");
    }
}