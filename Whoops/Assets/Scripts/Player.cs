using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;

    [SerializeField] private Animator revAnim;
    private Vector3 startPos;

    [SerializeField] private float spikeDmg;
    [SerializeField] private float maxHealth;
    [SerializeField] public float health;

    [SerializeField] private float drownDamage;
    public bool underWater = false;
    private bool drowning = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        healthBar.gameObject.SetActive(false);
        health = maxHealth;
        healthBar.SetMaxHealh(maxHealth);
    }

    private void Update()
    {
        Vector2 myPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2f);
        Vector2 wantedPos = Camera.main.WorldToScreenPoint(myPos);
        healthBar.gameObject.transform.position = wantedPos;

        if (underWater && !drowning)
        {
            StartCoroutine(Drowning());
            drowning = true;
        }
        else
        {
            drowning = false;
        }
    }

    public void TakeDamage(float damage)
    {
        healthBar.gameObject.SetActive(true);
        health -= damage;
        healthBar.SetHealth(health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            TakeDamage(spikeDmg);
        }
        else if (collision.CompareTag("RevSpike"))
        {
            StartCoroutine(Reverse());
        }
    }

    IEnumerator Drowning()
    {
        yield return new WaitForSeconds(2);

        while (underWater)
        {
            Debug.Log("Drowning");
            TakeDamage(drownDamage);

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Reverse()
    {
        revAnim.gameObject.SetActive(true);
        GetComponent<PlayerMovement>().enabled = false;
        revAnim.SetTrigger("Rev");
        transform.position = startPos;

        yield return new WaitForSeconds(0.5f);

        revAnim.gameObject.SetActive(false);
        GetComponent<PlayerMovement>().enabled = true;
    }
}
