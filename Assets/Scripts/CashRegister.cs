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
        // TODO; lel c'est pas ça qui devrait se passer.
        GetComponent<SpriteRenderer>().transform.Rotate(new Vector3(0, 0, m_isOpen ? 90 : -90));
    }
}
