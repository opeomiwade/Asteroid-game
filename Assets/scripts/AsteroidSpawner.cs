using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public int _asteroidAmount = 5;
    public Vector2 direction;
    public object Asteroid { get; set; }


    private void Start()
    {
        SpawnAsteroids();
    }

    private void SpawnAsteroids()
    {
        // spawn asteroid amount of asteroids into teh game scene.
        for (int i = 0; i < _asteroidAmount; i++)
        { 
            Asteroid asteroid = Instantiate(asteroidPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(2, Screen.width) , Random.Range(2,Screen.height))), Quaternion.identity);
            asteroid.transform.SetPositionAndRotation(new Vector3(asteroid.transform.position.x , asteroid.transform.position.y,1), Quaternion.identity);
            asteroid.size = Random.Range(asteroid.minAsteroidSize, asteroid.maxAsteroidSize);
            direction = new Vector2(asteroid.transform.position.x, asteroid.transform.position.y);
            asteroid.SetDirection(direction); // set direction of asteroid.
        }
    }
}