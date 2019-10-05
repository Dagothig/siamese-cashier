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
        foreach (var data in m_articleList)
        {
            var article = Instantiate(m_articlePrefab).GetComponent<Article>();
            GameManager.s_instance.GetZone(eZones.Tray).MoveArticleHere(article);
        }
    }
}
