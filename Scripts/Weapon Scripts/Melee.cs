using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Melee module that can be added to any Object.
 * They will gain the ability to choose a slice effect and swing hitbox
 * This is where the swing spawning occurs.
 */
public class Melee : MonoBehaviour
{
    [SerializeField] GameObject owner;
    [SerializeField] public AimWeapon aimWeapon;
    [SerializeField] protected string name;
    [SerializeField] protected float weaponCooldown = 0.15f;
    [SerializeField] public bool weaponOnCooldown;
    [SerializeField] protected GameObject swish;
    [SerializeField] protected GameObject hitbox;
    [SerializeField] protected int damage;
    [SerializeField] protected bool spawnProjectile;

    public Color color;
    void Awake()
    {
        // Disable weapon slash initially until hit is triggered.
        swish.SetActive(false);
        // Set animator to Parent Object's Animator Component.
        aimWeapon = gameObject.transform.root.GetComponent<AimWeapon>();
        //playerAimWeapon.setAnimation("isAttackingMelee");

        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        if (aimWeapon != null)
            aimWeapon.setAnimation("KnifeSlash");
    }


    public void swing(Vector3 gunEndPointPosition, float angle, Vector3 aimDirection)
    {
        owner = gameObject.transform.root.gameObject;
        // On weapon cooldown? No Swing.
        if (weaponOnCooldown)
            return;

        //swish.SetActive(true);  make this better first
        
        StartCoroutine(waitWeaponSwing(gunEndPointPosition, angle, aimDirection));

        // Weapon cooldown in between shots
        StartCoroutine(waitWeaponCooldown());
    }


    /*
     * Getters
     */


    public float getWeaponCooldown()
    {
        return weaponCooldown;
    }

    public void setSpawnProjectile(bool info)
    {
        spawnProjectile = info;
    }


    /*
     * Courotines
     */

    public IEnumerator waitWeaponSwing(Vector3 gunEndPointPosition, float angle, Vector3 aimDirection)
    {
        // Don't cause infinite stall.
        if (spawnProjectile)
            yield break;

        aimWeapon.animator.SetBool("isAttackingMelee", true);

        yield return new WaitUntil(() => spawnProjectile == true);
        // Treating a melee attack as a stationary bullet.
        
        GameObject hitbox = Instantiate(this.hitbox.gameObject, aimWeapon.aimGunEndPointTransform.position, Quaternion.Euler(new Vector3(0, 0, aimWeapon.angle)));
        
        // Cache this info soon
        hitbox.GetComponent<Swing>().damage = this.damage;
        hitbox.GetComponent<Swing>().owner = gameObject.transform.root.gameObject;
        spawnProjectile = false;
    }
    public IEnumerator waitWeaponCooldown()
    {
        weaponOnCooldown = true;
        
        yield return new WaitForSeconds(weaponCooldown);
        aimWeapon.animator.SetBool("isAttackingMelee", false);
        weaponOnCooldown = false;
    }

}
