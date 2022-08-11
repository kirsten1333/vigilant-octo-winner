using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GlobalTimer : MonoBehaviour
{
    //ТАЙМЕР
    public bool isGameStopped = false;
    public float timer = 0;
    private IEnumerator coroutine;

    [SerializeField]
    private GameObject timerUI;

    [SerializeField]
    public float seconds = 1;

    MobController mob;

    IEnumerator globalTimer()
    {
        while (true)
        {
            timer++;
            //mob.PlayerTakeDamage();
            yield return new WaitForSeconds(seconds);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mob = FindObjectOfType<MobController>();
        coroutine = globalTimer(); //Объявление таймера
        Invoke("Croutine", 1);
        

    }

    // Update is called once per frame
    void Update()
    {
        //КОНТРОЛЬ ТАЙМЕРА
        if (Input.GetKeyUp("space") && isGameStopped == false)
        {
            Debug.Log("TimerCount: " + (timer));
            StopCoroutine(coroutine);
            isGameStopped = true;
            Debug.Log(isGameStopped);
        }
        if (Input.GetKeyUp(KeyCode.Q) && isGameStopped == true)
        {
            Debug.Log("TimerCount: " + (timer));
            StartCoroutine(coroutine);
            isGameStopped = false;
            Debug.Log(isGameStopped);
        }
        timerUI.GetComponent<TextMeshProUGUI>().text = Convert.ToString(timer);
    }
    void Croutine()
    {
        StartCoroutine(coroutine); //Запуск таймер
    }
    public void StopTimer()
    {
        Debug.Log("TimerCount: " + (timer));
        StartCoroutine(coroutine);
        isGameStopped = false;
        Debug.Log(isGameStopped);
    }
}