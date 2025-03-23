using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCardManager : MonoBehaviour
{
    public static PowerUpCardManager instance { get; private set; }
    
    [Header("COMMON VALUES")]
    public float commonSpeedMultiplier;
    public float commonDamageMultiplier;
    public float commonCritChanceMultiplier;
    public float commonCritDamageMultiplier;
    public float commonAttackSpeedMultiplier;
    public float commonMaxHP;
    public float pickupRangeMultiplier;

    [Header("RARE VALUES")]
    public float xpGainMultiplier;
    public float knockbackMultiplier;

    public float rareSpeedMultiplier;
    public float rareDamageMultiplier;
    public float rareCritChanceMultiplier;
    public float rareCritDamageMultiplier;
    public float rareAttackSpeedMultiplier;
    public float rareMaxHP;
    public float rangeMultiplier;

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

    private void Start()
    {
        foreach(var card in ExpUI_Manager.instance.powerUpCards)
        {
            var cardSC = card.GetComponent<PowerUpCardSC>();
            cardSC.commonSpeedMultiplier = commonSpeedMultiplier;
            cardSC.commonDamageMultiplier = commonDamageMultiplier;
            cardSC.commonCritChanceMultiplier = commonCritChanceMultiplier;
            cardSC.commonCritDamageMultiplier = commonCritDamageMultiplier;
            cardSC.commonAttackSpeedMultiplier = commonAttackSpeedMultiplier;
            cardSC.pickupRangeMultiplier = pickupRangeMultiplier;
            cardSC.commonMaxHP = commonMaxHP;
            
            cardSC.xpGainMultiplier = xpGainMultiplier;
            cardSC.knockbackMultiplier = knockbackMultiplier;

            cardSC.rareSpeedMultiplier = rareSpeedMultiplier;
            cardSC.rareDamageMultiplier = rareDamageMultiplier;
            cardSC.rareCritChanceMultiplier = rareCritChanceMultiplier;
            cardSC.rareCritDamageMultiplier = rareCritDamageMultiplier;
            cardSC.rareAttackSpeedMultiplier = rareAttackSpeedMultiplier;
            cardSC.rareMaxHP = rareMaxHP;
            cardSC.rangeMultiplier = rangeMultiplier;
        }

    }

    public void CreateCards_LevelUp()
    {
        for(int i = 0; i < 3; i++)
        {
            var script = ExpUI_Manager.instance.powerUpCards[i].GetComponent<PowerUpCardSC>();
            script.SetUpCard(2);
            script.gameObject.SetActive(true);
        }
    }
}
