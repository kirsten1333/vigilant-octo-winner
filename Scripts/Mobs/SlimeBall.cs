using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBall : MonoBehaviour
{
    PlayerController playerController;
    public GameObject self;
    Animator animator;
    public float speed = 0.001f;
    public int damage = 5;
    float position = 0f;
    bool HasAttack = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        //damage = FindObjectOfType<Mobs>().damage; ------------------—делать определение урона автоматом? Ќадо ли, если у каждого монстра будет свой снар€д
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((self.transform.position - player.transform.position).sqrMagnitude < 0.25 && !HasAttack)
        {
            playerController.TakeDamage(damage);
            animator.SetTrigger("Destroy");
            HasAttack = true;
        }
        self.transform.position = Vector3.Lerp(
            self.transform.position, 
            player.transform.position,
            position);
        position += speed;
    }
    void DealDamage()
    {
        Destroy(self);
    }

}
