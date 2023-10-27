using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Utill
{
    public static List<T> GetShuffledList<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i);

            T value = list[i];
            list[i] = list[rand];
            list[rand] = value;
        }
        return list;
    }
}
