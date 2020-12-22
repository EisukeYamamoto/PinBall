using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitAction_Target_origin: MonoBehaviour
{

    public ParticleSystem Particle01;
    public ParticleSystem Particle02;
    HPSystem hpsys;

    private void Start()
    {
        hpsys = GetComponent<HPSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mallet"))
        {
            if (hpsys.hp <= 1)
            {
                Instantiate(Particle02, transform.position, Quaternion.identity);
            }
            else
            {
                Vector2 particlePos;
                particlePos.x = (collision.transform.position.x + transform.position.x) / 2;
                particlePos.y = (collision.transform.position.y + transform.position.y) / 2;

                Instantiate(Particle01, particlePos, Quaternion.identity);
            }
        }
        
        
        //transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f, 10, 0);
    }
}
