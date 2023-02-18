using System;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SpaceshipMovement : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private bool _flying;
    private float _rotationDirection;
    public float spaceshipSpeed = 2.5f;
    public float rotationSpeed = 0.5f;
    private bool _stopFlying;
    public Bullet bulletPrefab;
    public AudioSource bulletSound;
    public AudioSource spaceshipDies;
    public bool respawned = false;

    private void Start()
    {
        bulletSound = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        _flying = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        
        _stopFlying = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _rotationDirection = - 1.0f;
        }
        
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _rotationDirection = 1.0f;
        }
        
        else
        { 
            // ensure rotation stops after the keys have been released.
            _rotationDirection = 0.0f;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bulletSound.Play();
            ShootBullet();
        }
    }
    
    private void FixedUpdate()
    {
        if (_flying)
        {
            _rb2D.AddForce(transform.up * spaceshipSpeed);
        }
        
        else if (_rotationDirection != 0.0f)
        {
            _rb2D.AddTorque(_rotationDirection * rotationSpeed);
        }

        else if(_stopFlying)
        {
            // add same force in opposite direction to slow down spacehsip.
            var oppositeForce = -(transform.up * spaceshipSpeed);
            _rb2D.AddForce(oppositeForce); 
        }
    }

    private void ShootBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.projectBullet(transform.up);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("asteroid") && FindObjectOfType<Asteroid>()._powerUp == false)
        {
            _rb2D.velocity = Vector2.zero;
            _rb2D.angularVelocity = 0.0f;
            gameObject.SetActive(false);
            RespawnPlayer();
        }
    }
    
    public void RespawnPlayer()
    {
        //reset player score when they lose
        FindObjectOfType<GameManager>().score = 0;
        FindObjectOfType<GameManager>().scoreText.text = "0 POINTS"; 
        
        transform.SetPositionAndRotation(new Vector3( 0, 0, 1), Quaternion.identity);
        gameObject.layer = LayerMask.NameToLayer("StopCollsion");
        gameObject.SetActive(true);
        Invoke(nameof(AllowCollision), 2.0f); // make player invulnerable right after respawning for some time, in case asteroid is next to spawn.
        respawned = true;
    }

    
    private void AllowCollision()
    {
       gameObject.layer = LayerMask.NameToLayer("Bullet");
    }
}