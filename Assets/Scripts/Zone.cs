using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<Article> m_currentArticles;

    public void MoveArticleHere(Article article)
    {
        if (article.m_currentZone != null)
        {
            article.m_currentZone.RemoveArticle(article);
        }
        m_currentArticles.Add(article);
        article.transform.position = transform.position;
        article.m_currentZone = this;
    }

    public void RemoveArticle(Article article)
    {
        if (m_currentArticles.Contains(article))
        {
            m_currentArticles.Remove(article);
        }
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