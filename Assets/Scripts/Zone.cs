using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<Article> m_currentArticles;
    public Collider2D m_bounds;

    void Awake()
    {
        m_bounds = GetComponent<Collider2D>();
        if (m_bounds == null)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var bounds = gameObject.AddComponent<BoxCollider2D>();
            bounds.bounds.Encapsulate(spriteRenderer.bounds.min);
            bounds.bounds.Encapsulate(spriteRenderer.bounds.max);
            m_bounds = bounds;
        }
    }

    public virtual void MoveArticleHere(Article article)
    {
        if (article.m_currentZone != null)
        {
            article.m_currentZone.RemoveArticle(article);
        }

        article.transform.position = SetPositionOfArticle(article.m_articleData.radius);
        article.m_currentZone = this;
        m_currentArticles.Add(article);
    }

    protected Vector3 SetPositionOfArticle(float radius)
    {
        var position = m_bounds.bounds.center;
        var minX = m_bounds.bounds.min.x + radius;
        var minY = m_bounds.bounds.min.y + radius;
        var maxX = m_bounds.bounds.max.x - radius;
        var maxY = m_bounds.bounds.max.y - radius;
        for (var i = 1f; i >= 0; i -= 0.1f)
        {
            var r = radius * i;
            for (var j = 0; j < 100; j++)
            {
                position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
                if (!m_bounds.OverlapPoint(position))
                    continue;
                if (m_currentArticles.Any(a => (a.transform.position - position).sqrMagnitude < r + a.m_articleData.radius * i))
                    continue;
                return position;
            }
        }
        return position;
    }

    public virtual void RemoveArticle(Article article)
    {
        if (m_currentArticles.Contains(article))
        {
            m_currentArticles.Remove(article);
        }
    }

    public Article GetArticle(eArticles article)
    {
        foreach (var arti in m_currentArticles)
        {
            if (arti.m_articleData.article == article)
            {
                return arti;
            }
        }
        return null;
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