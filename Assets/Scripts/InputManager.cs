using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public List<sDoubleInteraction> m_interactionList;
    public List<eCashierActions> m_mappedActions;
    public bool m_inputSet;

    void Update()
    {
        if (Input.GetKey("up"))
        {
            if (Input.GetKey("w"))
            {
                if (!m_inputSet)
                {
                    GetOrSetInteraction(interactionsP1.W, interactionsP2.Up);
                }
                m_inputSet = true;
                return;
            }
        }
        m_inputSet = false;
    }

    eCashierActions GetOrSetInteraction(interactionsP1 p1Action, interactionsP2 p2Action)
    {
        foreach (var interaction in m_interactionList)
        {
            if (p1Action == interaction.p1Input && p2Action == interaction.p2Input)
            {
                //Already Mapped
                GameManager.s_instance.MakeAction(interaction.action);

                return interaction.action;
            }
        }
        var action = GameManager.s_instance.m_availableActions[0];
        MapAction(p1Action, p2Action, action);
        GameManager.s_instance.MakeAction(action);
        return action;
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