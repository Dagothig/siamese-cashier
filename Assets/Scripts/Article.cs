﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Article : MonoBehaviour
{
    public ArticleData m_articleData;

    public void MoveArticle(Zone zone)
    {
        zone.MoveArticleHere(this);
    }
}
