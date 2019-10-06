using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Client : Zone
{
    public List<ArticleData> m_articleList;
    public ArticleData m_payment;
    public GameObject m_articlePrefab;
    public float m_patienceTime;

    void Start()
    {
        GenerateGrocery();
    }

    void GenerateGrocery()
    {
        List<Article> articles = new List<Article>();
        foreach (var data in m_articleList.Take(3))
        {
            var article = Instantiate(m_articlePrefab).GetComponent<Article>();
            article.GetComponent<SpriteRenderer>().sprite = data.sprite;
            article.m_articleData = data;
            article.m_processingList = new List<eCashierActions>(data.processingList);
            articles.Add(article);
        }

        var payment = Instantiate(m_articlePrefab).GetComponent<Article>();
        payment.GetComponent<SpriteRenderer>().sprite = m_payment.sprite;
        payment.m_articleData = m_payment;
        payment.m_processingList = new List<eCashierActions>(m_payment.processingList);

        GameManager.s_instance.ResetClient(this, articles, payment);
    }
}
