using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayData", menuName = "DayData")]
public class DayData : ScriptableObject
{
    public List<List<eArticles>> clients;
}
