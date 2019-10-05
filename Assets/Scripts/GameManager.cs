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

    public void MakeAction(sCashierAction action)
    {
        if (action.eAction == eCashierActions.Grab)
        {
            //The article is on the tray and you pressed to grab it!
            var picked = GetZone(eZones.Tray).GetArticle(action.eArticle);
            if (picked != null)
            {
                GetZone(eZones.Hand).MoveArticleHere(picked);
                picked.m_processingList.RemoveAt(0);
            }
        }
        if (action.eAction == eCashierActions.Scan)
        {
            var picked = GetZone(eZones.Hand).GetArticle();
            if (picked != null)
            {
                GetZone(eZones.Bag).MoveArticleHere(picked);
                picked.m_processingList.RemoveAt(0);
            }
        }
    }

    public void ResetClient(Client client, List<Article> articles)
    {
        foreach (var article in articles)
        {
            GetZone(eZones.Tray).MoveArticleHere(article);
        }
    }

    public sCashierAction GetNextActionToMap(IList<sDoubleInteraction> interactions)
    {
        var hand = GetZone(eZones.Hand);
        var handArticle = hand.GetArticle();
        if (handArticle?.m_processingList.Any() ?? false)
        {
            var action = handArticle.m_processingList.First();
            var article = handArticle.m_articleData.article;
            if (!interactions.Any(i => i.action.eAction == action && i.action.eArticle == article))
            {
                return new sCashierAction 
                { 
                    eAction = action, 
                    eArticle = handArticle.m_articleData.article 
                };
            }
        }

        var tray = GetZone(eZones.Tray);
        var unmappedTrayArticles = 
            tray.m_currentArticles.Where(a => 
                !interactions.Any(i => 
                    i.action.eAction == eCashierActions.Grab && 
                    a.m_articleData.article == i.action.eArticle));
        var trayArticle = unmappedTrayArticles.FirstOrDefault();
        if (trayArticle?.m_processingList.Any() ?? false)
        {
            return new sCashierAction
            {
                eAction = trayArticle.m_processingList.First(),
                eArticle = trayArticle.m_articleData.article
            };
        }

        return (sCashierAction)m_currentClient.m_processingList.DefaultIfEmpty(eCashierActions.Count).FirstOrDefault();
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