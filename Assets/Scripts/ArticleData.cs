using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArticleData", menuName = "ArticleData")]
public class ArticleData : ScriptableObject
{
    public eArticles article;
    public int price;
    public float radius;
    public List<eCashierActions> processingList;
}
public enum eArticles
{
    Beer,
    Count
}