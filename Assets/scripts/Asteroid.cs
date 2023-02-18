using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Vector2 direction;
    private Vector3 lastVelocity;
    private Rigidbody2D rb2D;
    public float _asteroidSpeed = 50.0f;
    public float size = 1.0f;
    public float maxAsteroidSize = 2.0f;
    public float minAsteroidSize = 0.5f;
    public int split = 2;
    public bool notCalled = true;
    public bool _powerUp;
    
    private void Start()
    {
        if(notCalled){ // boolean to prevent method from being called when collisions are being handled.
            size = Random.Range(minAsteroidSize, maxAsteroidSize); //randomise size of each asteroid
            rb2D.mass = size;; // set mass according to size of object.
            transform.localScale = Vector3.one * size; //set scale according to mass assigned to asteroid
            SetDirection(Random.insideUnitCircle * 5); // set direction of asteroid.
            notCalled = false;
        }
    }

    private void Update()
    {
        lastVelocity = rb2D.velocity;
        
        if (FindObjectOfType<SpaceshipMovement>().respawned != true && Time.time > 5.0f)
        {//if player last first 5second of game without dying,spaceship acts as bullet.
            _powerUp = true;
            FindObjectOfType<GameManager>().PowerUpText.text = "POWER UP"; //let player know about powerup
            
        }
        Invoke(nameof(TurnPowerUpOff), 15.0f);
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        rb2D.AddForce(direction * _asteroidSpeed);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("upperwall") || col.gameObject.CompareTag("lowerwall") ||
            col.gameObject.CompareTag("leftwall") || col.gameObject.CompareTag("rightwall") || col.gameObject.CompareTag("asteroid"))
        {
          
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rb2D.AddForce(direction * _asteroidSpeed);
        }
        
        else if (col.gameObject.CompareTag("bullet")) 
        {
            if (size * 0.5f >= minAsteroidSize)
            {
                //split asteroid split times.
                for (int i = 0; i < split; i++)
                {
                    SplitAsteroid();
                }
            }
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().ChangeScore(this); // update player score when asteroid is destroyed.
        }
        
        else if (col.gameObject.CompareTag("Player") && _powerUp)
        {
            if (size * 0.5f >= minAsteroidSize)
            {
                //split asteroid split times.
                for (int i = 0; i < split; i++)
                {
                    SplitAsteroid();
                }
            }
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().ChangeScore(this); // update player score when asteroid is destroyed.
            
        }
    }

    private void TurnPowerUpOff()
    {
        _powerUp = false;
        FindObjectOfType<GameManager>().PowerUpText.text = "POWER UP GONE";
    }
    private void SplitAsteroid()
    {
        // slightly change asteroid position when spawning new half asteroids.
        Vector3 position = new Vector3( transform.position.x, transform.position.y,1);
        position += Random.insideUnitSphere * 0.5f;
        
        //instantiate new half asteroid and set size
        Asteroid asteroid = Instantiate(this, position, transform.rotation);
        asteroid.size = size * 0.5f ;
        asteroid.transform.localScale = Vector3.one * asteroid.size; // scale asteroid accoring to the size.
        
        // set direction of new asteroid within a circle of radius 1 so half asteroid doesnt spawn and hit spacehip.
        asteroid.SetDirection(Random.insideUnitCircle * 2);
    }
}