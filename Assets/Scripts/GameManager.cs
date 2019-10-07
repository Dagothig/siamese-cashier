using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public List<sGroceryArticle> m_groceryStoreArticles;
    public GameObject m_clientPrefab;
    public List<sZones> m_zoneDictionnary;
    public List<List<ArticleData>> m_clientGroceries;
    public int m_score;

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
        var client = GetZone(eZones.Client) as Client;
        var cashRegister = GetZone(eZones.CashRegister) as CashRegister;

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
                    cashRegister?.m_invoice.Add(picked.m_articleData);
                }
                GetZone(eZones.Bag).MoveArticleHere(picked);
                picked.m_processingList.RemoveAt(0);
            }
        }
        if (action.eAction == eCashierActions.CashRegister)
        {
            if (cashRegister != null)
            {
                var hand = GetZone(eZones.Hand);
                var picked = hand.GetArticle();
                if (picked != null)
                {
                    if (cashRegister.m_isOpen)
                    {
                        // We've put the whatever inside the register.
                        picked.m_processingList.RemoveAt(0);
                        cashRegister.MoveArticleHere(picked);
                    }
                    else
                    {
                        // We tried to put the payment in a closed register, so we drop it.
                        hand.RemoveArticle(picked);
                        Destroy(picked.gameObject);
                    }
                }
                
                cashRegister.Toggle();
            }
        }
        if (action.eAction == eCashierActions.Client)
        {
            var tray = GetZone(eZones.Tray);
            var hand = GetZone(eZones.Hand);

            var picked = hand.GetArticle();
            if (picked != null)
            {
                // We've given the client whatever thingy we had in hand.
                client.MoveArticleHere(picked);
            }
            else
            {
                // We've taken the payment from the client.
                var payment = tray.m_currentArticles.Any() ? null : client.GetArticle(eArticles.Payment);
                if (payment != null)
                {
                    hand.MoveArticleHere(payment);
                }
            }
        }

        // If the client has given their payment and the cash register is closed, then they are done here
        if (client.GetArticle(eArticles.Payment) == null && !(cashRegister?.m_isOpen ?? false))
        {
            client.Leave();
            ResetClient(client, newArticles, newPayments);
        }
    }

    public void ResetClient(Client client, List<Article> articles, Article payment)
    {
        foreach (var article in articles)
        {
            GetZone(eZones.Tray).MoveArticleHere(article);
        }
        GetZone(eZones.Client).MoveArticleHere(payment);
    }

    private sCashierAction getNextActionToMapForZoneArticles(IList<sDoubleInteraction> interactions, Zone zone)
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

        return (sCashierAction)eCashierActions.Count;
    }

    public sCashierAction GetNextActionToMap(IList<sDoubleInteraction> interactions)
    {
        var cashRegisterIsOpen = (GetZone(eZones.CashRegister) as CashRegister)?.m_isOpen ?? false;
        return new eZones[] { eZones.Hand, eZones.Tray }
            .Select(GetZone)
            .Select(zone => getNextActionToMapForZoneArticles(interactions, zone))
            .Where(action => action.eAction != eCashierActions.Count)
            .DefaultIfEmpty(
                cashRegisterIsOpen ? 
                    getNextActionToMapForZoneArticles(interactions, GetZone(eZones.Client)) : 
                    (sCashierAction)eCashierActions.CashRegister)
            .FirstOrDefault();
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