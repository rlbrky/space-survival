using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCardSC : MonoBehaviour
{
    [SerializeField] Image cardHeaderImage;
    [SerializeField] TextMeshProUGUI cardHeaderText;
    [SerializeField] TextMeshProUGUI cardBodyText;

    private int _cardEffect;
    private float _effectValue;

    public float commonSpeedMultiplier;
    public float commonDamageMultiplier;
    public float commonCritChanceMultiplier;
    public float commonCritDamageMultiplier;
    public float commonAttackSpeedMultiplier;
    public float commonMaxHP;
    public float pickupRangeMultiplier;

    //RARE POWERUPS.
    public float xpGainMultiplier;
    public float knockbackMultiplier;

    public float rareSpeedMultiplier;
    public float rareDamageMultiplier;
    public float rareCritChanceMultiplier;
    public float rareCritDamageMultiplier;
    public float rareAttackSpeedMultiplier;
    public float rareMaxHP;
    public float rangeMultiplier;

    private bool isKnockbackPicked;
    private float percentage;

    private void OnEnable()
    {
        if (PlayerTouchInputs.instance != null)
        {
            PlayerTouchInputs.instance.DisableTouchStuff();
        }
    }

    private void OnDisable()
    {
        if (PlayerTouchInputs.instance != null)
        {
            PlayerTouchInputs.instance.EnableTouchStuff();
        }
    }

    //Card Rarity can be 1 or 2 -- 1 means rare - 2 means common

    public void SetUpCard(int _cardRarity)
    {
        if(_cardRarity == 1)
        {
            Color _color;

           if(UnityEngine.ColorUtility.TryParseHtmlString("#640055", out _color))
                cardHeaderText.color = _color;

            int random = Random.Range(0, MassCardManager.instance.rareCardValues.Count);
            int switchHandler = MassCardManager.instance.rareCardValues[random];
            MassCardManager.instance.rareCardValues.Remove(switchHandler);
            switch (switchHandler)
            {
                case 0:
                    percentage = (xpGainMultiplier * 100) - 100;
                    cardHeaderText.text = "READING SKILL!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> more <color=#ADD8E6>xp</color> per drop.";
                    _cardEffect = 7;
                    _effectValue = xpGainMultiplier;
                    break;
                case 1:
                    cardHeaderText.text = "MULTI SHOTS!";
                    cardBodyText.text = "<color=black>Add more spell per shot.</color> ";
                    _cardEffect = 8;
                    break;
                case 2:
                    cardHeaderText.text = "BECOME A VAMPIRE!";
                    cardBodyText.text = "<color=red>Gain Life Steal!</color>";
                    _cardEffect = 9;
                    break;
                case 3:
                    cardHeaderText.text = "KNOCK KNOCK!";
                    if (!isKnockbackPicked)
                        cardBodyText.text = "<color=black>Gain Knockback!</color> ";
                    else
                    {
                        percentage = (knockbackMultiplier * 100) - 100;
                        cardBodyText.text = "Get <color=orange>%" + percentage.ToString("0.0") + "</color> <color=black>knockback strength</color>" ;
                    }
                    _cardEffect = 10;
                    _effectValue = knockbackMultiplier;
                    break;
                case 4:
                    cardHeaderText.text = "MORE SPELLS!";
                    cardBodyText.text = "Gain an additional <color=black>spell caster!</color>";
                    _cardEffect = 11;
                    break;
                case 5:
                    percentage = (rareSpeedMultiplier * 100) - 100;
                    cardHeaderText.text = "RUSH SCROLL!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=#00FFFF>movement speed!</color>";
                    _cardEffect = 12;
                    _effectValue = rareSpeedMultiplier;
                    break;
                case 6:
                    percentage = (rareDamageMultiplier * 100) - 100;
                    cardHeaderText.text = "MIGHT SCROLL!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=red>damage.</color>";
                    _cardEffect = 13;
                    _effectValue = rareDamageMultiplier;
                    break;
                case 7:
                    percentage = (rareCritChanceMultiplier * 100) - 100;
                    cardHeaderText.text = "LUCK MASTERY SCROLL!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=#FF00FF>crit chance.</color>";
                    _cardEffect = 14;
                    _effectValue = rareCritChanceMultiplier;
                    break;
                case 8:
                    percentage = (rareCritDamageMultiplier * 100) - 100;
                    cardHeaderText.text = "CATASTROPHIC SPELLS!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=orange>crit damage.</color>";
                    _cardEffect = 15;
                    _effectValue = rareCritDamageMultiplier;
                    break;
                case 9:
                    percentage = (rareAttackSpeedMultiplier * 100) - 100;
                    cardHeaderText.text = "CAST MASTER!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=yellow>attack speed.</color>";
                    _cardEffect = 16;
                    _effectValue = rareAttackSpeedMultiplier;
                    break;
                case 10:
                    percentage = (rareMaxHP * 100) - 100;
                    cardHeaderText.text = "HEARTY SCROLL!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=green>health</color> and <color=green>heal</color> for <color=orange>%20</color>";
                    _cardEffect = 17;
                    _effectValue = rareMaxHP;
                    break;
                case 11:
                    percentage = (rangeMultiplier * 100) - 100;
                    cardHeaderText.text = "EAGLE SCROLL!";
                    cardBodyText.text = "Get <color=#3f000f>%" + percentage.ToString("0.0") + "</color> <color=#800000>range</color> for your spells.";
                    _cardEffect = 18;
                    _effectValue = rangeMultiplier;
                    break;
            }
        }
        else
        {
            cardHeaderText.color = new Color(0, 0, 155); //Blue color for common card.
            int random = Random.Range(0, MassCardManager.instance.commonCardValues.Count);
            int switchHandler = MassCardManager.instance.commonCardValues[random];
            MassCardManager.instance.commonCardValues.Remove(switchHandler);
            switch (switchHandler)
            {
                case 0:
                    percentage = (commonSpeedMultiplier * 100) - 100;
                    cardHeaderText.text = "SPRINT SCROLL!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=#00FFFF>movement speed.</color>";
                    _cardEffect = 0;
                    _effectValue = commonSpeedMultiplier;
                    break;
                case 1:
                    percentage = (commonDamageMultiplier * 100) - 100;
                    cardHeaderText.text = "STRENGTH SCROLL!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=red>damage.</color>";
                    _cardEffect = 1;
                    _effectValue = commonDamageMultiplier;
                    break;
                case 2:
                    percentage = (commonCritChanceMultiplier * 100) - 100;
                    cardHeaderText.text = "LUCKY SCROLL!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=#FF00FF>crit chance.</color>";
                    _cardEffect = 2;
                    _effectValue = commonCritChanceMultiplier;
                    break;
                case 3:
                    percentage = (commonCritDamageMultiplier * 100) - 100;
                    cardHeaderText.text = "ENRAGED SCROLL!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=orange>crit damage.</color>";
                    _cardEffect = 3;
                    _effectValue = commonCritDamageMultiplier;
                    break;
                case 4:
                    percentage = (commonAttackSpeedMultiplier * 100) - 100;
                    cardHeaderText.text = "CASTING SCROLL!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=yellow>attack speed.</color>";
                    _cardEffect = 4;
                    _effectValue = commonAttackSpeedMultiplier;
                    break;
                case 5:
                    percentage = (commonMaxHP * 100) - 100;
                    cardHeaderText.text = "HEALTH SCROLL!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=green>health.</color> and <color=green>heal</color> for <color=#3EB49B>%10</color>";
                    _cardEffect = 5;
                    _effectValue = commonMaxHP;
                    break;
                case 6:
                    percentage = (pickupRangeMultiplier * 100) - 100;
                    cardHeaderText.text = "LONGER ARMS!";
                    cardBodyText.text = "Get <color=#3EB49B>%" + percentage.ToString("0.0") + "</color> <color=#DF7474>pickup range.</color>";
                    _cardEffect = 6;
                    _effectValue = pickupRangeMultiplier;
                    break;
            }
        }
    }

    public void ApplyCardEffect()
    {
        MassCardManager.instance.ResetCards();
        Time.timeScale = 1f;
        //PlayerSC.instance.enabled = true;
        ExpUI_Manager.instance.characterExpUI.value = 0;
        foreach (var card in ExpUI_Manager.instance.powerUpCards)
            card.gameObject.SetActive(false);
        switch (_cardEffect)
        {
            case 0: //Movement Speed - DONE.
                PlayerSC.instance.speed *= _effectValue;

                GameManager.instance.expNgemTravelSpeed *= _effectValue;
                break;
            case 1: //Basic Damage - DONE.
                PlayerSC.instance.damage *= commonDamageMultiplier;
                break;
            case 2: //Crit Chance - DONE.
                PlayerSC.instance.critChance *= commonCritChanceMultiplier;
                break;
            case 3: //Crit Damage - DONE.
                PlayerSC.instance.critDamageMultiplier *= commonCritDamageMultiplier;
                break;
            case 4: //Attack Speed - DONE.
                PlayerSC.instance.fireRate /= commonAttackSpeedMultiplier;
                break;
            case 5: //Max Health - DONE.
                PlayerSC.instance.maxHealth *= _effectValue;
                PlayerSC.instance.health += PlayerSC.instance.maxHealth / 10;
                PlayerSC.instance.healthbar.maxValue = PlayerSC.instance.maxHealth;
                PlayerSC.instance.healthbar.value = PlayerSC.instance.health;
                break;
            case 6: //PICKUP RADIUS - DONE.
                GameManager.instance.expNgemPickupRadius *= _effectValue;
                break;
            case 7: //MORE EXP - DONE.
                GameManager.instance.expValue *= _effectValue;
                break;
            case 8: //MULTISHOT - DONE.
                //Multi shot can only be picked twice, after that it will no longer spawn.
                if (MassCardManager.instance.multiShotFirstPicked)
                {
                    MassCardManager.instance.multiShotEnd = true;
                    for (int i = 0; i < PlayerSC.instance.spells.Length; i++)
                    {
                        PlayerSC.instance.spells[i].gameObject.SetActive(true);
                        PlayerSC.instance.spells[i]._bullet = Resources.Load("TRIPLE SHOT") as GameObject;
                        PlayerSC.instance.spells[i].gameObject.SetActive(false);
                    }
                    for (int i = 0; i < PlayerSC.instance.activeSpellCount; i++)
                        PlayerSC.instance.spells[i].gameObject.SetActive(true);
                    
                }
                else
                {
                    MassCardManager.instance.multiShotFirstPicked = true;
                    for (int i = 0; i < PlayerSC.instance.spells.Length; i++)
                    {
                        PlayerSC.instance.spells[i].gameObject.SetActive(true);
                        PlayerSC.instance.spells[i]._bullet = Resources.Load("DOUBLE SHOT") as GameObject;
                        PlayerSC.instance.spells[i].gameObject.SetActive(false);
                    }
                    for (int i = 0; i < PlayerSC.instance.activeSpellCount; i++)
                        PlayerSC.instance.spells[i].gameObject.SetActive(true);
                }
                break;
            case 9: //LIFE STEAL - DONE.
                PlayerSC.instance.isLifeStealing = true;
                MassCardManager.instance.noMoreLifeSteal = true;
                break;
            case 10: // Knockback - DONE.
                if(isKnockbackPicked)
                {
                    PlayerSC.instance.knockbackForce *= _effectValue;
                }
                else
                {
                    isKnockbackPicked = true;
                    PlayerSC.instance.hasKnockback = true;
                }
                break;
            case 11: //GET MORE spells - DONE.
                switch(PlayerSC.instance.activeSpellCount)
                {
                    case 1:
                        PlayerSC.instance.spells[1].gameObject.SetActive(true);
                        PlayerSC.instance.activeSpellCount++;
                        break;
                    case 2:
                        PlayerSC.instance.spells[2].gameObject.SetActive(true);
                        PlayerSC.instance.activeSpellCount++;
                        break;
                    case 3:
                        PlayerSC.instance.spells[3].gameObject.SetActive(true);
                        PlayerSC.instance.activeSpellCount++;
                        MassCardManager.instance.noMoreDrones = true;
                        break;
                }
                break;
            case 12: // RARE SPEED - DONE.
                PlayerSC.instance.speed *= _effectValue;

                GameManager.instance.expNgemTravelSpeed *= _effectValue;
                break;
            case 13: //RARE DAMAGE - DONE.
                PlayerSC.instance.damage *= rareDamageMultiplier;
                break;
            case 14: //RARE CRIT CHANCE - DONE.
                PlayerSC.instance.critChance *= rareCritChanceMultiplier;
                break;
            case 15: // RARE CRIT DAMAGE - DONE
                PlayerSC.instance.critDamageMultiplier *= rareCritDamageMultiplier;
                break;
            case 16: //RARE ATTACK SPEED - DONE.
                PlayerSC.instance.fireRate /= rareAttackSpeedMultiplier;
                break;
            case 17: //Rare Max Health - DONE.
                PlayerSC.instance.maxHealth *= _effectValue;
                PlayerSC.instance.health += PlayerSC.instance.maxHealth / 5;
                PlayerSC.instance.healthbar.maxValue = PlayerSC.instance.maxHealth;
                PlayerSC.instance.healthbar.value = PlayerSC.instance.health;
                break;
            case 18: //GET RANGE - DONE.
                if (PlayerSC.instance.range <= 30f)
                {
                    PlayerSC.instance.range *= _effectValue;
                }
                else
                    MassCardManager.instance.cantGetMoreRange = true;
                break;
        }
    }
}
