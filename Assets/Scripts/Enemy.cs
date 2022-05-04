using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public int health;
    public float speed;
    public float normalSpeed;
    public int damage;

    public float startTimeBtwAttack;
    public float startStopTime;

    public GameObject floatingDamage;
    public GameObject damageEffect;

    private float timeBtwAttack;
    private float stopTime;
    private Girl girl;
    private Animator anim;
    private int look;
    private AddRoom room;
    private int sceneNumber;

    [SerializeField] bool redEnemy;
    [SerializeField] bool bossEnemy;

    private void Start()
    {
        anim = GetComponent<Animator>();
        girl = FindObjectOfType<Girl>();
        room = GetComponentInParent<AddRoom>();
        timeBtwAttack = startTimeBtwAttack;
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if(stopTime <= 0)
        {
            speed = normalSpeed;
        }
        else
        {
            speed = 0;
            stopTime -= Time.deltaTime;
        }

        if(health <= 0)
        {
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
            if (bossEnemy)
            {
                StartCoroutine(C_WaitForSeconds());
                if (sceneNumber < 10)
                {
                    SceneManager.LoadScene(sceneNumber + 1);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

        if (girl.transform.position.x > transform.position.x && look==0)
        {
            look = 1;
            transform.Rotate(new Vector3(0, 180, 0));
        }
        else if (girl.transform.position.x < transform.position.x && look == 1)
        {
            look = 0;
            transform.Rotate(new Vector3(0, 180, 0));
        }

        transform.position += ((girl.transform.position - transform.position).normalized * speed * Time.deltaTime);
        
    }

    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        Instantiate(damageEffect, transform.position, Quaternion.identity);
        health -= damage;
        Vector2 damagePos = new Vector2(transform.position.x, transform.position.y + 1.75f);
        Instantiate(floatingDamage, damagePos, Quaternion.identity);
        floatingDamage.GetComponentInChildren<FloatingDamage>().damage = damage;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("AttackTrigger"))
        {
            if (timeBtwAttack <= 0 && redEnemy)
            {
                anim.SetTrigger("Red_attack");
            }
            else if (timeBtwAttack <= 0 && bossEnemy)
            {
                anim.SetTrigger("BossAttack");
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    public void OnEnemyAttack()
    {
        girl.ChangeHealth(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }

    private IEnumerator C_WaitForSeconds()
    {
        Debug.Log("Start to wait " + 3 + " seconds");
        yield return new WaitForSeconds(3);
        Debug.Log("Counting finished");
    }
}
