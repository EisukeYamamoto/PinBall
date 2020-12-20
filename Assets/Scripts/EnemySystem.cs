using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    CircleCollider2D collider;
    [SerializeField]
    private float invincibleTime = 3;
    public Vector2 targetPos;
    public Vector2 initPos;

    // Start is called before the first frame update
    void Start()
    {
        collider = this.gameObject.GetComponent<CircleCollider2D>();
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        collider.enabled = false;

        yield return new WaitForSeconds(invincibleTime);
        yield return new WaitForEndOfFrame();

        collider.enabled = true;
    }

}
