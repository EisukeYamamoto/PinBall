using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HitAction : MonoBehaviour
{

    public ParticleSystem Particle;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mallet"))
        {
            Instantiate(Particle, transform.position, Quaternion.identity);
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f, 10, 0);
        }
        
    }
}
