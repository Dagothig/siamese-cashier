using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Zone
{
    public bool m_hasObject;

    public override void MoveArticleHere(Article article)
    {
        if (m_hasObject)
        {
            Destroy(m_currentArticles[0].gameObject);
            m_currentArticles.Clear();
        }
        if (article.m_currentZone != null)
        {
            article.m_currentZone.RemoveArticle(article);
        }
        article.transform.position = SetPositionOfArticle(article.m_articleData.radius);
        article.m_currentZone = this;
        m_currentArticles.Add(article);
        m_hasObject = true;
    }

    public override void RemoveArticle(Article article)
    {
        base.RemoveArticle(article);
        m_hasObject = false;
    }
}
