
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1000.0f;
    private Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // method to project bullet in direction given multiplied by the speed.
   public void projectBullet(Vector2 direction)
    {
        rb2D.AddForce(direction * bulletSpeed);
    }

   private void OnCollisionEnter2D(Collision2D col)
   {
       Destroy(gameObject); // destroy bullet on collision with another gameobject collider. 
   }
   
  
}
