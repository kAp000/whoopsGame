using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    public GameObject escMenu;
    bool open = false;

    public Text deahText;
    public Timer timer;
    public GameObject deathUi;
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
        escMenu.SetActive(false);
        timer.StartTimer();
        startPos = transform.position;
        healthBar.gameObject.SetActive(false);
        health = maxHealth;
        healthBar.SetMaxHealh(maxHealth);
    }

    private void Update()
    {
        if(health <= 0)
        {
            timer.EndTimer();
            Death();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Esc();
        }

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

    public void Esc()
    {
        if (open)
        {
            GetComponent<PlayerMovement>().enabled = true;
            timer.StartTimer();
            escMenu.SetActive(false);
            open = false;
        }
        else if (!open)
        {
            GetComponent<PlayerMovement>().enabled = false;
            escMenu.SetActive(true);
            timer.EndTimer();
            open = true;
        }
    }

    public void Death()
    {
        FindObjectOfType<AudioManager>().Play("Death");
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().Sleep();
        GetComponent<PlayerMovement>().enabled = false;
        TimeSpan time = TimeSpan.FromSeconds(timer.currentTime);
        deahText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        deathUi.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Vic");
        gameObject.GetComponent<Player>().enabled = false;
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
        FindObjectOfType<AudioManager>().Play("Reverse");
        revAnim.gameObject.SetActive(true);
        GetComponent<PlayerMovement>().enabled = false;
        revAnim.SetTrigger("Rev");
        transform.position = startPos;

        yield return new WaitForSeconds(0.5f);

        revAnim.gameObject.SetActive(false);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerMovement>().isJumping = false;
    }
}
