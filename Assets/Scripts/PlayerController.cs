//AI was used to help with some adujments to the code, but the majority of the code was written by me.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    public GameObject boosterFlame;
    public GameObject explosionEffect;

    public float scoreMultiplier = 10f;
    private float elapsedTime = 0f;
    private float score = 0f;
    private bool isGameOver = false;

    public UIDocument uiDocument;
    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (uiDocument == null) return;

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");

        if (restartButton != null)
        {
            restartButton.style.display = DisplayStyle.None;
            restartButton.clicked += ReloadScene;
        }

        if (highScoreText != null)
        {
            highScoreText.style.display = DisplayStyle.None;
            highScoreText.style.transitionProperty = new StyleList<StylePropertyName>(new List<StylePropertyName> { "opacity" });
            highScoreText.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new TimeValue(0.5f, TimeUnit.Second) });
        }
    }

    void Update()
    {
        if (isGameOver) return;

        UpdateScore();
        MovePlayer();
    }

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void MovePlayer()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && boosterFlame != null)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame && boosterFlame != null)
        {
            boosterFlame.SetActive(false);
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
    
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shields") && !isGameOver)
        {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        isGameOver = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        if (boosterFlame != null) boosterFlame.SetActive(false);
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        
        if (rb != null) rb.simulated = false; 

        float currentHighScore = PlayerPrefs.GetFloat("PlayerHighScore", 0f);

        if (highScoreText != null)
        {
            highScoreText.style.display = DisplayStyle.Flex;

            if (score > currentHighScore)
            {
                PlayerPrefs.SetFloat("PlayerHighScore", score);
                PlayerPrefs.Save();

                highScoreText.text = "NEW HIGH SCORE: " + score;
                highScoreText.style.color = Color.green;
                highScoreText.style.opacity = 0f;
                highScoreText.schedule.Execute(() => { highScoreText.style.opacity = 1f; }).StartingIn(50);
            }
            else
            {
                highScoreText.text = "Best Score: " + currentHighScore;
                highScoreText.style.color = Color.white;
            }
        }

        if (restartButton != null)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
