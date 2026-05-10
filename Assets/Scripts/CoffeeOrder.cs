using System.Collections.Generic;

[System.Serializable]
public class CoffeeOrder
{
    public string coffeeType;
    public int sugarLevel;

    // ?? TEK AROMA SÝLÝNDÝ
    // public string aroma;

    // ?? YENÝ SÝSTEM
    public List<string> aromas;
}