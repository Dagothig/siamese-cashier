using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public List<sGroceryArticle> m_groceryStoreArticles;
    public Client m_currentClient;
    public GameObject m_clientPrefab;
    public List<sZones> m_zoneDictionnary;
    public List<sCashierAction> m_availableActions;


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

    public void MakeAction(eCashierActions action, eArticles article)
    {
        if (action == eCashierActions.Grab)
        {
            //The article is on the tray and you pressed to grab it!
            var picked = GetZone(eZones.Tray).GetArticle(article);
            if (picked != null)
                GetZone(eZones.Hand).MoveArticleHere(picked);
        }
    }

    public void ResetClient(Client client, List<Article> articles)
    {
        m_availableActions.Clear();
        foreach (var article in articles)
        {
            GetZone(eZones.Tray).MoveArticleHere(article);
            m_availableActions.AddRange(
                article.m_articleData.processingList
                .Select(a => 
                    new sCashierAction { 
                        eAction = a, 
                        eArticle = a == eCashierActions.Grab ? article.m_articleData.article : eArticles.Count 
                    }));
        }
        m_availableActions.Add((sCashierAction)eCashierActions.CashRegister);
        m_availableActions.Add((sCashierAction)eCashierActions.Client);
        m_availableActions.Add((sCashierAction)eCashierActions.CashRegister);
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

[System.Serializable]
public struct sCashierAction
{
    public eCashierActions eAction;
    public eArticles eArticle;

    public static explicit operator sCashierAction(eCashierActions action) => 
        new sCashierAction { eAction = action, eArticle= eArticles.Count };
}