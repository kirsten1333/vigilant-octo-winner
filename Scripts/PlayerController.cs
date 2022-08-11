using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class PlayerController : MonoBehaviour
{
    public int maxHealth; //Максимальное (начальное) значение здоровья моба
    public int health; // Текущее здоровье моба
    public Slider slider;
    public Animator animator;
    public GameObject deathScreen;
    public GameObject timerUI;
    public GameObject timer; //Экземпляр таймера на сцене
    float time;

    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
        deathScreen.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("TakeDamage");
        health -= damage;
        slider.value = health;
        Debug.Log("player take " + damage + " damage");
        if (health < 0)
        {
            time = timer.GetComponent<GlobalTimer>().timer;
            DeathScreen(time);
        }
    }
    public void Heal(int heal)
    {
        if (health < maxHealth)
        {
            Debug.Log("player heal " + heal + " damage");
            if (health + heal > maxHealth) { health = maxHealth; }
            else { health += heal; }
            slider.value = health;
        }
    }
    public void DeathScreen(float timer)
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
        timerUI.GetComponent<TextMeshProUGUI>().text = Convert.ToString(timer);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


