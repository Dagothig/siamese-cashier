﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SiameseCashier;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public List<sGroceryArticle> m_groceryStoreArticles;
    public Client m_currentClient;
    public GameObject m_clientPrefab;
    public List<sZones> m_zoneDictionnary;

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (m_currentClient == null)
        {
            m_currentClient = Instantiate(m_clientPrefab, transform).GetComponent<Client>();
        }
    }

    public Zone GetZone(eZones eZone)
    {
        foreach (var zone in m_zoneDictionnary)
        {
            if (zone.eZone == eZone)
            {
                return zone.zone;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct sGroceryArticle
{
    public eArticles article;
    public ArticleData articleData;
}

[System.Serializable]
public struct sZones
{
    public eZones eZone;
    public Zone zone;
}