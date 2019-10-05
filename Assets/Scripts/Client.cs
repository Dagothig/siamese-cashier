using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public List<ArticleData> m_articleList;
    public GameObject m_articlePrefab;
    public float m_patienceTime;

    void Start()
    {
        GenerateGrocery();
    }

    void GenerateGrocery()
    {
        List<Article> articles = new List<Article>();
        foreach (var data in m_articleList)
        {
            articles.Add(Instantiate(m_articlePrefab).GetComponent<Article>());
        }
        GameManager.s_instance.ResetClient(this, articles);

    }
}
