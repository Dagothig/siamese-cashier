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
        if (action.eAction == eCashierActions.Scan || action.eAction == eCashierActions.Scale)
        {
            var picked = GetZone(eZones.Hand).GetArticle();
            if (picked != null)
            {
                if (picked.m_processingList[0] == action.eAction)
                {
                    var cashRegister = GetZone(eZones.CashRegister) as CashRegister;
                    cashRegister?.m_invoice.Add(picked.m_articleData);
                }
                GetZone(eZones.Bag).MoveArticleHere(picked);
                picked.m_processingList.RemoveAt(0);
            }
        }
        if (action.eAction == eCashierActions.CashRegister)
        {
            var cashRegister = GetZone(eZones.CashRegister) as CashRegister;
            if (cashRegister != null)
            {
                if (cashRegister.m_isOpen)
                {
                    var picked = GetZone(eZones.Hand).GetArticle();
                    picked.m_processingList.RemoveAt(0);
                    cashRegister.MoveArticleHere(picked);
                }
                cashRegister.Toggle();
            }
        }
        if (action.eAction == eCashierActions.Client)
        {
            var tray = GetZone(eZones.Tray);
            var client = GetZone(eZones.Client);
            var hand = GetZone(eZones.Hand);

            var picked = hand.GetArticle();
            if (picked != null)
            {
                client.MoveArticleHere(picked);
            }
            else
            {
                var payment = tray.m_currentArticles.Any() ? null : client.GetArticle(eArticles.Payment);
                if (payment != null)
                {
                    hand.MoveArticleHere(payment);
                }
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
        var zones = new eZones[] { eZones.Hand, eZones.Tray }.Select(GetZone);
        foreach (var zone in zones)
        {
            var unmappedArticles =
            zone.m_currentArticles.Where(a =>
                !interactions.Any(i =>
                    i.action.eAction == a.NextAction() &&
                    (i.action.eArticle == eArticles.Count || a.m_articleData.article == i.action.eArticle)));
            var article = unmappedArticles.FirstOrDefault();
            if (article != null)
            {
                var action = article.m_processingList.First();
                return new sCashierAction
                {
                    eAction = action,
                    eArticle = action == eCashierActions.Grab ? article.m_articleData.article : eArticles.Count
                };
            }
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