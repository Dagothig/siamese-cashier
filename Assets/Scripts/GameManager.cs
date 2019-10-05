using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public List<sGroceryArticle> m_groceryStoreArticles;
    public Client m_currentClient;
    public GameObject m_clientPrefab;
    public List<sZones> m_zoneDictionnary;
    public List<eCashierActions> m_availableActions;


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
        if (m_currentClient == null || m_availableActions.Count == 0)
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

    public void MakeAction(eCashierActions action)
    {
        if (action == eCashierActions.Grab1)
        {
            //There is article 1 on the tray and you pressed grab article 1!
            if (m_availableActions.Contains(action))
            {
                var beer = GetZone(eZones.Tray).GetArticle(eArticles.Beer);
                GetZone(eZones.Hand).MoveArticleHere(beer);
                m_availableActions.Remove(eCashierActions.Grab1);
            }
            //Dude, you don't have article 1 on the tray!
            else
            {

            }
        }
    }

    public void ResetClient(Client client, List<Article> articles)
    {
        m_availableActions.Clear();
        foreach (var article in articles)
        {
            GetZone(eZones.Tray).MoveArticleHere(article);
            m_availableActions.AddRange(article.m_articleData.processingList);
        }
        m_availableActions.Add(eCashierActions.CashRegister);
        m_availableActions.Add(eCashierActions.Client);
        m_availableActions.Add(eCashierActions.CashRegister);
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