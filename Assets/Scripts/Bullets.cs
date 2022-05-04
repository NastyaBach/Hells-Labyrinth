using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid; // Что пуля будет считать твёрдым и будет пробивать

    [SerializeField] bool enemyBullet;

    private void Start()
    {
        Invoke("DestroyBullet", lifetime);
    }

    private void Update()
    {
        // Находить объект для пробития с помощью RaycastHit2D
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null) // Если пуля столкнулась с коллайдером
        {
        
            if(hitInfo.collider.CompareTag("Enemy")) // Если у него тэг Enemy
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage); // Наносим урон
            }
           
            if (hitInfo.collider.CompareTag("Player") && enemyBullet) // Если у него тэг Enemy
            {
                hitInfo.collider.GetComponent<Girl>().ChangeHealth(-damage); // Наносим урон
            }
            DestroyBullet();
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime); // С помощью чего патрон движется
    }

    public void DestroyBullet()
    {
        Destroy(gameObject); // Уничтожаем патрон
    }
}
