using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour
{
    public List<string> unlockedAromas = new List<string>();

    public void CheckUnlock(int level)
    {
        if (level == 2 && !unlockedAromas.Contains("Fýndýk"))
            unlockedAromas.Add("Fýndýk");

        if (level == 3 && !unlockedAromas.Contains("Fýstýk"))
            unlockedAromas.Add("Fýstýk");

        if (level == 4 && !unlockedAromas.Contains("Tarçýn"))
            unlockedAromas.Add("Tarçýn");

        Debug.Log("Unlocked aromalar: " + string.Join(",", unlockedAromas));
    }
}