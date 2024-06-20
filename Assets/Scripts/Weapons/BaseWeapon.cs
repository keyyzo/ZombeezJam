using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Base Weapon Paramenters")]
    [SerializeField] private float weaponDamageMultiplier = 1f;
    [SerializeField] private float rateOfFire = 1f;
    private float bulletTimer = 0f;
    private bool isShooting = false; // not needed on the editor currently

    [Header("Ammo Parameters")]
    [SerializeField] private GameObject bulletType;
    [SerializeField] private int maxNumOfBulletsInClip = 0;
    [SerializeField] private int maxNumOfBulletsToCarry = 0;
    [SerializeField] private int currentNumOfBulletsInClip = 1;
    [SerializeField] private int currentNumOfBulletsCarried = 0;

    [Header("Bullet Spawn Parameters")]
    [SerializeField] private Transform bulletSpawnPos;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (isShooting)
        {
            bulletTimer += Time.deltaTime;
        }

        if (bulletTimer >= rateOfFire)
        {
            isShooting = false;
            bulletTimer = 0.0f;
        }
    }

    public void FireWeapon(float bullletDirection)
    {
        if (currentNumOfBulletsInClip > 0 && isShooting != true)
        {
           

            GameObject tempBullet = Instantiate(bulletType, bulletSpawnPos.position, Quaternion.identity);
            isShooting = true;

            tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(tempBullet.GetComponent<BaseBullet>().GetSpeed() * bullletDirection, 0.0f);

            if (bullletDirection < 0)
            { 
                tempBullet.GetComponent<SpriteRenderer>().flipX = true;
            }

            else if (bullletDirection > 0)
            {
                tempBullet.GetComponent<SpriteRenderer>().flipX = false;

            }

        }
    }

    public void ReloadWeapon()
    { 
        
    }

    public float CalculateDamage()
    {
        float damageToGive = 0;




        return damageToGive;
    }

    void ChamberNextBullt()
    { 
        
    }

    
}
