using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat armor;
    public Stat damage;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Damage Test
        {
            takeDamage(10);
        }
    }

    public void takeDamage(int damage)
    {
        //damage *= 1 - ();
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        /**
         * The character will die in some way
         * The method of death depends on who it is
        **/
        Debug.Log(transform.name + " has been slain.");
    }
}
