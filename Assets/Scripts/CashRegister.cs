using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : Zone
{
    public bool m_isOpen;
    public List<ArticleData> m_invoice = new List<ArticleData>();
    public List<ArticleData> m_finalInvoice = new List<ArticleData>();

    public void Toggle()
    {
        if (!m_isOpen)
        {
            m_finalInvoice = m_invoice;
        }
        m_isOpen = !m_isOpen;
        m_invoice = new List<ArticleData>();
        // TODO; lel c'est pas ça qui devrait se passer.
        GetComponent<SpriteRenderer>().transform.Rotate(new Vector3(0, 0, m_isOpen ? 90 : -90));
    }

    public override void MoveArticleHere(Article article)
    {
        base.MoveArticleHere(article);
        if (article.m_articleData.article == eArticles.Payment)
            GameManager.s_instance.m_score += m_finalInvoice.Sum(x => x.price);
    }
}
