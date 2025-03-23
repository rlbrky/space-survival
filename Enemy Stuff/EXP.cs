using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    public float expValue = 5f;
    public float speed = 15f;

    public SphereCollider expDropCollider;


    private float lifeTime = 30f;
    private float counter = 0;


    private bool headToPlayer;
    private Transform player;

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= lifeTime)
        {
            Destroy(gameObject);
        }

        if (headToPlayer)
        {
            transform.LookAt(player);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            PlayerSC.instance.pickupSFX.Play();
            PlayerSC.instance.exp += expValue;
            ExpUI_Manager.instance.characterExpUI.value = PlayerSC.instance.exp;
            ExpUI_Manager.instance.playerText.text = PlayerSC.instance.exp.ToString("0") + " / " + PlayerSC.instance.expThreshold;
            if (PlayerSC.instance.exp >= PlayerSC.instance.expThreshold)
            {
                PlayerSC.instance.LevelUP();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            headToPlayer = true;
            player = other.transform;
        }
    }
}
