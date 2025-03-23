using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SHOPSC : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI shopText;
    [Header("Values")]
    [SerializeField] private float keyThreshold;
    [SerializeField] private float keyThresholdScale;
    [SerializeField] private float transferSpeed;
    //[SerializeField] private AudioSource sfx;

    private float keys = 0f;
    private float shopTextKeyCount;

    private void Start()
    {
        slider.maxValue = 1;
        shopTextKeyCount = 0;
        shopText.text = keys.ToString("0") + " / " + keyThreshold.ToString("0");
    }

    private void Update()
    {
        if (keys >= keyThreshold)
        {
          slider.value = 0;
          Time.timeScale = 0;
          keys -= keyThreshold;
          keyThreshold += keyThresholdScale;
          //slider.maxValue = keyThreshold;
          for (int i = 0; i < 3; i++)
          {
              var script = ExpUI_Manager.instance.powerUpCards[i].GetComponent<PowerUpCardSC>();
              script.SetUpCard(1);
              script.gameObject.SetActive(true);
          }
        }
        else if(slider.value != keys / keyThreshold)
        {
            //sfx.Play();
            slider.value = Mathf.Lerp(slider.value, keys / keyThreshold, transferSpeed * Time.deltaTime);
            shopTextKeyCount = Mathf.Lerp(shopTextKeyCount, keys, transferSpeed * Time.deltaTime);
            shopText.text = shopTextKeyCount.ToString("0") + " / " + keyThreshold.ToString("0");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (PlayerSC.instance.keys > 0)
            {
                keys += PlayerSC.instance.keys;
                PlayerSC.instance.keys = 0;
                ExpUI_Manager.instance.keyText.text = "Keys: " + PlayerSC.instance.keys;
                //slider.value = keys / keyThreshold;
            }
        }
    }
}
