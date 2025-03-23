using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCardManager : MonoBehaviour
{
    public static MassCardManager instance { get; private set; }

    List<PowerUpCardSC> powerUpCards;

    public bool multiShotFirstPicked;
    public bool multiShotEnd;

    public bool noMoreDrones;
    public bool noMoreLifeSteal;
    public bool cantGetMoreRange;

    public List<int> commonCardValues= new List<int>();
    public List<int> rareCardValues= new List<int>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 7; i++)
            commonCardValues.Add(i);

        for(int i = 0; i < 12; i++)
            rareCardValues.Add(i);

        powerUpCards = new List<PowerUpCardSC>();
        foreach (var powerUpCard in ExpUI_Manager.instance.powerUpCards)
        {
            powerUpCards.Add(powerUpCard.GetComponent<PowerUpCardSC>());
        }
    }

    public void ResetCards()
    {
        commonCardValues.Clear();
        rareCardValues.Clear();

        for (int i = 0; i < 7; i++)
            commonCardValues.Add(i);

        for (int i = 0; i < 12; i++)
            rareCardValues.Add(i);

        if (multiShotEnd)
            rareCardValues.Remove(1);

        if (noMoreDrones)
            rareCardValues.Remove(4);

        if (noMoreLifeSteal)
            rareCardValues.Remove(2);

        if(cantGetMoreRange)
            rareCardValues.Remove(11);
    }
}
