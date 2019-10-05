using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public List<ArticleData> m_articleList;
    public GameObject m_articlePrefab;
    public float m_patienceTime;
    public List<eCashierActions> m_processingList;

    void Start()
    {
        GenerateGrocery();
    }

    void GenerateGrocery()
    {
        List<Article> articles = new List<Article>();
        foreach (var data in m_articleList)
        {
            var article = Instantiate(m_articlePrefab).GetComponent<Article>();
            article.GetComponent<SpriteRenderer>().sprite = data.sprite;
            article.m_articleData = data;
            article.m_processingList = new List<eCashierActions>(data.processingList);
            articles.Add(article);
        }
        GameManager.s_instance.ResetClient(this, articles);
        m_processingList = new List<eCashierActions> 
        { 
            eCashierActions.CashRegister, 
            eCashierActions.Client,
            eCashierActions.CashRegister 
        };
    }
}
