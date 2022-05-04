using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GunType gunType;
    public float offset; // Эту величину задаём самостоятельно, чтобы контролировать вращение
    public Joystick joystick;
    public GameObject bullet;
    public Transform shotPoint;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private Vector3 difference;
    private float rotZ;
    public enum GunType {Defaul, Enemy}

    private Girl girl;

    private void Start()
    {
        girl = GameObject.FindGameObjectWithTag("Player").GetComponent<Girl>();
        if (girl.controlType == Girl.ControlType.PC && gunType == GunType.Defaul)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update() 
    {
        if (gunType == GunType.Defaul)
        {
            if (girl.controlType == Girl.ControlType.PC)
            {
                difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }

            else if (girl.controlType == Girl.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f)
            {
                rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
            }
        }
        
        else if(gunType == GunType.Enemy)
        {
            difference = girl.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset); 
        // Рассчитываем позиции курсора мыши и заставляем пушку вращаться в его направлении

        if (timeBtwShots <= 0) 
        {
            if (Input.GetMouseButtonDown(0) && girl.controlType == Girl.ControlType.PC || gunType == GunType.Enemy) //Если нажата левая кнопка мыши
            {
                Shoot();
            }
            else if (girl.controlType == Girl.ControlType.Android)
            {
                if (joystick.Horizontal != 0 || joystick.Vertical != 0)
                {
                    Shoot();
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        timeBtwShots = startTimeBtwShots;
    }
}
