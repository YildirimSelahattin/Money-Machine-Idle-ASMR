 using UnityEngine;
 using System.Collections.Generic;
 using System.Linq;

public static class AbbrevationUtility
 {
     private static readonly SortedDictionary<long, string> abbrevations = new SortedDictionary<long, string>
     {
         {1000," k"},
         {1000000, " m" },
         {1000000000, " b" },
         {1000000000000, " t" }
     };
 
     public static string AbbreviateNumber(float number)
     {

        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<long, string> pair = abbrevations.ElementAt(i);
            if (Mathf.Abs(number) >= pair.Key)
            {
                float roundedNumber = Mathf.FloorToInt(number / pair.Key);
                return roundedNumber.ToString() + pair.Value;
            }
        }
        return number.ToString();
    }

    public static string AbbreviateNumberForTotalMoney(float number) {
        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<long, string> pair = abbrevations.ElementAt(i);
            if (Mathf.Abs(number) >= pair.Key)
            {
                if (Mathf.Abs(number) > 1000000)
                {
                    float roundedNumber = Mathf.FloorToInt(number / pair.Key);
                    return roundedNumber.ToString() + pair.Value;
                }
                else
                {
                    return number.ToString()+pair.Value;
                }
            }
        }
        return number.ToString() ;

    }
 }