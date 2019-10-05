using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    public List<sDoubleInteraction> m_interactionList;
    public List<eCashierActions> m_mappedActions;

    public string[] m_keysP1;
    public string[] m_keysP2;

    public bool m_inputSet;

    void Awake()
    {
        m_keysP1 = Enum.GetNames(typeof(interactionsP1)).Take((int)interactionsP1.Count).Select(x => x.ToLower()).ToArray();
        m_keysP2 = Enum.GetNames(typeof(interactionsP2)).Take((int)interactionsP2.Count).Select(x => x.ToLower()).ToArray();
    }

    void Update()
    {
        for (var keyP1 = 0; keyP1 < m_keysP1.Length; keyP1++)
        {
            if (!Input.GetKey(m_keysP1[keyP1]))
                continue;
            for (var keyP2 = 0; keyP2 < m_keysP2.Length; keyP2++)
            {
                if (!Input.GetKey(m_keysP2[keyP2]))
                    continue;

                if (!m_inputSet)
                    GetOrSetInteraction((interactionsP1)keyP1, (interactionsP2)keyP2);
                m_inputSet = true;
                return;
            }
        }
        m_inputSet = false;
    }

    void GetOrSetInteraction(interactionsP1 p1Action, interactionsP2 p2Action)
    {
        foreach (var interaction in m_interactionList)
        {
            if (p1Action == interaction.p1Input && p2Action == interaction.p2Input)
            {
                //Already Mapped
                GameManager.s_instance.MakeAction(interaction.action);
                return;
            }
        }
        if (GameManager.s_instance.m_availableActions.Count == 0)
            return;

        var action = GameManager.s_instance.m_availableActions[0];
        MapAction(p1Action, p2Action, action);
        GameManager.s_instance.MakeAction(action);
    }

    void MapAction(interactionsP1 p1Action, interactionsP2 p2Action, eCashierActions action)
    {
        var interaction = new sDoubleInteraction() { p1Input = p1Action, p2Input = p2Action, action = action} ;
        m_interactionList.Add(interaction);
    }
}

[System.Serializable]
public struct sDoubleInteraction
{
    public interactionsP1 p1Input;
    public interactionsP2 p2Input;
    public eCashierActions action;
}

public enum interactionsP1
{
    W,
    A,
    S,
    D,
    Count
}

public enum interactionsP2
{
    Up,
    Down,
    Left,
    Right,
    Count
}