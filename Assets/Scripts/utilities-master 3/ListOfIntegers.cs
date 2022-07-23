using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ListOfIntegers{
	

	public static List<int> GenerateRandom(int count, int min, int max)
        {
	        if (max <= min || count < 0 ||
                    (count > max - min && max - min > 0))
            {
	    		throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
		        " (" + ((int)max - (int)min) + " values), or count " + count + " is illegal");
        	}
 
            HashSet<int> candidates = new HashSet<int>();
 
            for (int top = max - count; top < max; top++)
            {
	            if (!candidates.Add(UnityEngine.Random.Range(min, top + 1)))
                {
                    candidates.Add(top);
                }
            }
 
	        List<int> result = new List<int>(candidates);
 
            for (int i = result.Count - 1; i > 0; i--)
            {
	            int k = UnityEngine.Random.Range(min,i + 1);
                int tmp = result[k];
                result[k] = result[i];
                result[i] = tmp;
            }
            return result;
        }
 
}
