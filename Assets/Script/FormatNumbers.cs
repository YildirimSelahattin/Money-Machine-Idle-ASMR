 using UnityEngine;
 using System.Collections.Generic;
 using System.Linq;
 
 public static class AbbrevationUtility
 {
     private static readonly SortedDictionary<int, string> abbrevations = new SortedDictionary<int, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" }
     };
 
     public static string AbbreviateNumber(float number)
     {
         for (int i = abbrevations.Count - 1; i >= 0; i--)
         {
             KeyValuePair<int, string> pair = abbrevations.ElementAt(i);
             if (Mathf.Abs(number) >= pair.Key)
             {
                 float roundedNumber =number / pair.Key;
                 return roundedNumber.ToString() + pair.Value;
             }
         }
         return number.ToString();
     }
 }