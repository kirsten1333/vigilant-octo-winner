using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mobs : MonoBehaviour
{

    public int maxHealth; //ћаксимальное (начальное) значение здоровь€ моба
    int health;
    public int damage; //Ќачальный урон моба
    public MobController mobController;
    public Slider slider;
    public int position;
    public GameObject self;
    public IEnumerator timer;
    public float attackSpeed;
    public bool isAttack = false;
    public bool isDead = false;
    public Animator animator;
    public PlayerController player;
    public typeMob type;
    public GameObject BallPrefab;


    void OnEnable()
    {
        mobController = FindObjectOfType<MobController>();
        timer = AttackTimer();
        animator = self.GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        slider.maxValue = maxHealth;
        health = maxHealth;
        slider.value = health;

    }
    public IEnumerator AttackTimer()
    {
        while (true)
        {
            try { if (!isDead) { Attack(); } } ///// ---------------------------------!!!!!!!!!!!!!!!!!!!!!
            catch { Debug.Log("err"); }
            //mob.PlayerTakeDamage();
            yield return new WaitForSeconds(attackSpeed);
        }
    }
    public void TakeDamage(int damage) 
    {
        health -= damage;
        slider.value = health;
        animator.SetTrigger("TakeDamage");
        if (health < 0 && isDead !=true)
        {
            isDead = true;
            animator.SetTrigger("Death");
        }
        Debug.Log("Mob on " + mobController.listOfSlimes.IndexOf(self) + " position takes: " + damage + " damage");
    }
    public void Attack()//ѕроблема последовательности спавна и активации атаки
    { //Ќужна ли проверка на смерть игрока? -------------------------------??????
        animator.SetTrigger("Attack");
        //player.TakeDamage(damage);
    }
    public void SpawnSlimeBall()
    {
        Instantiate(
            BallPrefab,
            self.transform.position,
            Quaternion.identity,
            GameObject.FindGameObjectWithTag("BG").transform);
    }
    public void Spawned()
    {
        Debug.Log("Slime was spawned");
        position = mobController.listOfSlimes.IndexOf(self);
        Debug.Log(position);
    }
    public void Death()
    {
        StopCoroutine(timer);
        GameObject d = mobController.listOfSlimes[0];
        mobController.listOfSlimes.Remove(gameObject);
        mobController.UpdatePosition();
        Destroy(gameObject);
    }
    public enum typeMob
    {
        slime, flower
    }
}
