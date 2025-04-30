using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage  
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private bool hit;
    private BoxCollider2D collision;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collision = GetComponent<BoxCollider2D>();
        collision.enabled = false;
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        collision.enabled = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if(hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if(lifetime > resetTime)
          {
            Deactivate();  
          }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);

        if(anim != null)
        {
          anim.SetTrigger("explode");
          Invoke("Deactivate", 0.5f);
        }
        else 
            Deactivate();
    }

    private void Deactivate()
    {
        collision.enabled = false;
        gameObject.SetActive(false);   
    }


}
