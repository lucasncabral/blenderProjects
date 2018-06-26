﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {
    Player playerEntitity;

    public Image fadePlane;
    public GameObject gameOverUI;

    public RectTransform newWaveBanner;
    public Text newWaveTitle;
    public Text newWaveEnemyCount;
    public Text scoreUI;
    public RectTransform healthBar;

    public Text gameOverScoreUI;

    Spawner spawner;
    // Use this for initialization
    void Start ()
    {
        playerEntitity = FindObjectOfType<Player>();
        Action OnPlayerDeathAction = () => OnGameOver();
        playerEntitity.OnDeath += OnPlayerDeathAction;
    }

    void Awake()
    {
        spawner = FindObjectOfType<Spawner>();

    }

    private void Update()
    {
        scoreUI.text = ScoreKeeper.score.ToString("D6");
        float healthPercent = 0;
        if(playerEntitity != null) { 
            healthPercent = playerEntitity.health / playerEntitity.startingHealth;
        }
        healthBar.localScale = new Vector3(healthPercent, 1, 1);
    }


    IEnumerator AnimateNewWaveBanner()
    {
        float delayTime = 1.5f;
        float speed = 2.5f;
        float animatePercent = 0;
        int dir = 1;

        float endDelayTime = Time.time + 1 / speed + delayTime;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * speed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                    dir = -1;
            }
            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-160, 0, animatePercent);
            yield return null;
        }
    }

    public void OnNewWave(int waveNumber)
    {
        string[] numbers = { "One", "Two", "Three", "Four", "Five" };
        newWaveTitle.text = "- Wave " + numbers[waveNumber - 1] + " -";
        string enemyCountString = ((spawner.waves[waveNumber - 1].infinit) ? "Infinite" : spawner.waves[waveNumber - 1].enemyCount + "");
        newWaveEnemyCount.text = "Enemies: " + enemyCountString;

        StopCoroutine("AnimateNewWaveBanner");
        StartCoroutine("AnimateNewWaveBanner");
    }


    void OnGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0,0,0,.90f), 1));
        gameOverScoreUI.text = scoreUI.text;
        scoreUI.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time){
        float speed = 1 / time;
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }


    // UI Input
    public void StartNewGame()
    {
        SceneManager.LoadScene("Main");
    }
}
