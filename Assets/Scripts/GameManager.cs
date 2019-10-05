using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SiameseCashier;

public class GameManager : MonoBehaviour
{
    public List<sGroceryArticle> m_groceryStoreArticles;

}

[System.Serializable]
public struct sGroceryArticle
{
    public eArticles article;
    public ArticleData articleData;
}