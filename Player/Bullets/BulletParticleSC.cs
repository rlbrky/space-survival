using UnityEngine;

public class BulletParticleSC : MonoBehaviour
{
    [SerializeField] float bulletLifeTime = 4f;

    float lifeTimeCounter = 0f;

    private void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= bulletLifeTime)
        {
            Destroy(transform.root.gameObject);
        }
    }
}
