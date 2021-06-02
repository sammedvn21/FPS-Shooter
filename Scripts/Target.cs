
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float health = 100f;
    public AudioSource explosionSound;
    public void TakeDamage(float amount,RaycastHit hit)
    {
        health -= amount;
        if(health<=0)
        {
            Die(hit);
        }
    }

    void Die(RaycastHit hit)
    {
        Instantiate(explosionPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        explosionSound.Play();
        Destroy(gameObject);
    }


}
