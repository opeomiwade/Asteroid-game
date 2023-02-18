
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int score = 0;
    public Text scoreText;
    public Text LevelLabel;
    public Text PowerUpText;
    public int maxLevel = 2;
    public Asteroid[] Asteroids { get ; private set; }
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += AfterLoadingLevel;
    }
    
    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        score = 0;
        LoadLevel(1);
        SetLevelLabel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;
        if (level > maxLevel)
        {
            this.level = 1;
            LoadLevel(1);
            return;
        }
        SceneManager.LoadScene("asteroidsLevel" + level);
    }
    
    
    //function called automatically after scene is loaded.
    private void AfterLoadingLevel(Scene scene, LoadSceneMode mode)
    {
        Asteroids = FindObjectsOfType<Asteroid>();
    }
    
    //changes score on hit of asteroid with bullet.
    public void ChangeScore(Asteroid asteroid)
    {
        UpdateScore(asteroid);
        if (CompletedLevel())
        {
            if ((GameObject.FindWithTag("asteroid") == null))
            {
                LoadLevel(level + 1);
                SetLevelLabel(level);
                score = 0;
                scoreText.text = score + " POINTS";
            }
        }
    }

    
    //checks if the player has destroyed every asteroid in the scene.
    private bool CompletedLevel()
    {
        for (int i = 0; i < Asteroids.Length; i++)
        {
            if (Asteroids[i].gameObject.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateScore(Asteroid asteroid)
    {
        if (asteroid.size <= 0.6f)
        { // 100 points for small asteroids as they are harder to hit.
            score += 15;
        }
        
        else if (asteroid.size >= 1.5f && asteroid.size <= 2.0f)
        { // large asteroids are the easiest to hit.
            score += 5;
        }

        else
        { // medium asteroids
            score += 10;
        }
        scoreText.text = score + " POINTS";
    }

    private void SetLevelLabel(int level)
    { // update level label when player beats the current level.
        LevelLabel.text = "LEVEL " + level;
    }

   
}
