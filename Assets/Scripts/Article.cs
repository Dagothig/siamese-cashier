using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Article : MonoBehaviour
{
    public ArticleData m_articleData;
    public List<eCashierActions> m_processingList;
    public Zone m_currentZone;

    public eCashierActions NextAction() =>
        m_processingList.DefaultIfEmpty(eCashierActions.Count).FirstOrDefault();
}
