using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : Zone
{
    public bool m_isOpen;
    public List<ArticleData> m_invoice = new List<ArticleData>();

    public void Toggle()
    {
        m_isOpen = !m_isOpen;
    }
}
