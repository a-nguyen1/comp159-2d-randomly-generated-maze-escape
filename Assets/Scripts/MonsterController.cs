using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private int detectionRange;
    [SerializeField] private float velocity;
    [SerializeField] private int maxStamina;

    private Transform target;

    private bool drainingStamina;
    
    private int currentStamina;
    
    private Rigidbody2D rb;

    private Vector2 move;
    
    private int adjustment;

    private bool touchingObstacle;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(target.position, transform.position) <= detectionRange)
        {
            // move towards player if within detection range
            move = (target.position - transform.position);
            if (touchingObstacle)
            {
                // if monster is colliding with wall/object, try to rotate
                double angleInRadians = Math.Atan2(move.y, move.x);
                adjustment += 1; // adjust angle to move
                angleInRadians += adjustment * Math.PI / 180;
                // if all 360 degrees have been tried, respawn the monster 
                if (adjustment > 360) 
                {
                    RespawnMonster();
                    
                }
                move.x = (float)Math.Cos(angleInRadians);
                move.y = (float)Math.Sin(angleInRadians);
            }
            else
            {
                adjustment = 0;
            }
            rb.velocity = move.normalized * velocity;
            StartCoroutine("DrainStamina");
        }
        else
        {
            rb.velocity = Vector2.zero;
            currentStamina = maxStamina;
        }
    }

    private void RespawnMonster()
    {
        // TODO change to spawn at proper spawn location
        transform.position = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over :(");
            // TODO implement game over screen from here
        }
        else
        {
            touchingObstacle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        touchingObstacle = false;
    }

    private IEnumerator DrainStamina()
    {
        if (!drainingStamina)
        {
            drainingStamina = true;
            yield return new WaitForSeconds(1);
            currentStamina = Mathf.Clamp(currentStamina - 1, 0, maxStamina);
            if (currentStamina == 0)
            {
                Debug.Log("Monster is tired :/");
                currentStamina = maxStamina;
                RespawnMonster(); // "respawn" the monster
            }
            drainingStamina = false;
        }
    }
    
    
}
