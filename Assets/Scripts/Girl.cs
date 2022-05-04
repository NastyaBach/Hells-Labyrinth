using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Girl : MonoBehaviour
{
    [Header("Controls")]
    public ControlType controlType;
    public Joystick joystick;
    public float speed;
    public enum ControlType { PC, Android };
    
    [Header("Health")]
    public int health;
    public Text healthDisplay;
    public GameObject damage_effect;
    public GameObject healing_effect;

    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;
    public GameObject shield_effect;

    [Header("Weapons")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponIcon;

    [Header("Key")]
    public GameObject keyIcon;
    public GameObject openDoorEffect;

    private Rigidbody2D rb;
    private Vector2 moveInput;  //Считывает, в какую сторону будет двигаться
    private Vector2 moveVelocity;  //Итоговая скорость в нужном направлении
    private Animator anim;
    private bool facingRight = true;
    private bool keyButtonPushed;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (controlType == ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()  
    {
        if (controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        
        moveVelocity = moveInput.normalized * speed; // Выдаёт конечную скорость

        if (moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }

        else if (facingRight && moveInput.x < 0)
        {
            Flip();
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    void FixedUpdate() // Двигает игрока
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void ChangeHealth(int healthValue)
    {
        if (!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0)
        {
            health += healthValue;
            healthDisplay.text = "HP: " + health;
            if (healthValue < 0)
            {
                Instantiate(damage_effect, transform.position, Quaternion.identity);
            }
        }
        else if (shield.activeInHierarchy && healthValue < 0)
        {
            shieldTimer.ReduceTime(healthValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Potion"))
        {
            Instantiate(healing_effect, other.transform.position, Quaternion.identity);
            ChangeHealth(5);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            Instantiate(shield_effect, other.transform.position, Quaternion.identity);
            if (!shield.activeInHierarchy)
            {
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                Destroy(other.gameObject);
            }
            else
            {
                shieldTimer.ResetTimer();
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Weapon"))
        {
            for (int i = 0; i < allWeapons.Length; i++)
            {
                if (other.name == allWeapons[i].name)
                {
                    unlockedWeapons.Add(allWeapons[i]);
                }
            }
            SwitchWeapon();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Key"))
        {
            keyIcon.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    public void OnKeyButtonDown()
    {
        keyButtonPushed = !keyButtonPushed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("LockedDoor") && keyButtonPushed && keyIcon.activeInHierarchy)
        {
            Instantiate(openDoorEffect, other.transform.position, Quaternion.identity);
            keyIcon.SetActive(false);
            other.gameObject.SetActive(false);
            keyButtonPushed = false;
        }
    }
    public void SwitchWeapon()
    {
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (unlockedWeapons[i].activeInHierarchy)
            {
                unlockedWeapons[i].SetActive(false);
                if (i != 0)
                {
                    unlockedWeapons[i - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;
                }
                weaponIcon.SetNativeSize();
                break;
            }
        }
    }
}
