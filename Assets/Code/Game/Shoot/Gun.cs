using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Custom/Gun", order = 2002)]
public class Gun : ScriptableObject
{
    public new string name = "";
    public Item item;
    public SpriteRenderer spriteRenderer;

    [Header("Projectile")]
    public GameObject projectile;
    public float projectileOffset = 0;
    public float projectileRate = 1;
    public int projectileDamage = 10;
    public float projectileSpeed = 1;
    public float projectileLifetime = 3;
    public bool destroyOnCollision = true;

    [Header("Effect")]
    public GameObject shootEffect;
    public float effectOffset = 0;
}