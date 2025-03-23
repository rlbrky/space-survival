using UnityEngine;

public class GemSC : MonoBehaviour
{
    [SerializeField] private float gemValue = 5f;
    public float speed = 15f;
    public SphereCollider gemDropCollider;

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

        if(headToPlayer )
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
            PlayerSC.instance.keys += gemValue;
            ExpUI_Manager.instance.keyText.text = "keys: " + PlayerSC.instance.keys;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            headToPlayer = true;
            player = other.transform;
        }
    }
}
