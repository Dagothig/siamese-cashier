using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<Article> m_currentArticles;

    public void MoveArticleHere(Article article)
    {
        m_currentArticles.Add(article);
        article.transform.position = transform.position;
    }
}


public enum eZones
{
    Tray,
    Bag,
    Scanner,
    Scale,
    Hand,
    Count
}