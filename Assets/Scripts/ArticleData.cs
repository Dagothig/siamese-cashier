using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SiameseCashier;

[CreateAssetMenu(fileName = "ArticleData", menuName = "ArticleData")]
public class ArticleData : ScriptableObject
{
    public eArticles article;
    public int price;
    public List<eCashierActions> processingList;
}

namespace SiameseCashier
{
    public enum eArticles
    {
        Banana,
        Count
    }
}