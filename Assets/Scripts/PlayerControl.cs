using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    
    public float force;
    public Rigidbody playerRB;

    public GUIStyle mystyle;

    public GameObject obstacleCollissionEffect; 

    public float health;
    public float highPoint;
    public bool goingDown;
    public float damage;

    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        mystyle.normal.textColor = Color.white;
        mystyle.fontSize = 16;
        startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            force += 20 * Time.deltaTime;

        }

        if (Input.GetMouseButtonUp(0))
        {
            // This will transform mouse position from screenlocation to 3D world location
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
            mousePos.z = 0;
            // Direction from mouse to the player is calculated using B-A vector locations
            Vector3 dir = (mousePos - transform.position).normalized;

            if(dir.y < 0)
            {
                dir *= -1;
            }

            Launch(force, dir);

        }

        // Let's check every frame when the ball start to come down.

        if(playerRB.linearVelocity.y < -0.01 && goingDown == false)
        {
            // this is the moment (frame) the ball is starting to come down
            GetComponent<Renderer>().material.color = Color.red;
            goingDown = true;
            highPoint = transform.position.y;

        }

    }

    void Launch(float forceAmt, Vector3 forceDir)
    {
        GetComponent<Renderer>().material.color = Color.white;
        playerRB.AddForce(forceDir * forceAmt, ForceMode.Impulse);
        force = 0;
        goingDown = false; 

    }

    void TakeDamage(float dmgTaken)
    {
        health -= dmgTaken;
        if(health < 0)
        {
            Die();
        }

    }

    void Die()
    {
        // Move player back to the startposition
        transform.position = startPosition;
        health = 100;
        playerRB.linearVelocity = Vector3.zero;
        // Let's take all platform to an array and go trough them with for each and change their color and layer to inactive. 
        GameObject[] allPlatforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach(GameObject singlePlatform in allPlatforms)
        {
            singlePlatform.layer = LayerMask.NameToLayer("PlatformInactive");
            //singlePlatform.GetComponent<Renderer>().material.color = Color.magenta;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Hit a Platform, let's take some damage");
            if(goingDown == true)
            {
                damage = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y)) * Mathf.Abs(highPoint - transform.position.y);
                TakeDamage(damage);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // If player object is colliding with an object that has a tag "Obstacle" on it. 
            // Instansiate the particle effect
            GameObject obsEffect = Instantiate(obstacleCollissionEffect, transform.position, Quaternion.identity);
            Destroy(obsEffect, 3);

            TakeDamage(20);
        }

        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            // This if run if player hits Level End.
            // Read NextLevel value from Level End Object's Level end script and open a scene named that. 
            SceneManager.LoadScene(collision.gameObject.GetComponent<LevelEnd>().nextLevel);

        }


    }



    private void OnGUI()
    {
        // this is just for debugging
        GUI.Label(new Rect(10, 10, 200, 20), "Force: " + force, mystyle);
        GUI.Label(new Rect(10, 30, 200, 20), "High Point: " + highPoint, mystyle);
        GUI.Label(new Rect(10, 50, 200, 20), "Going Down: " + goingDown, mystyle);
        GUI.Label(new Rect(10, 70, 200, 20), "Damage: " + damage, mystyle);
        GUI.Label(new Rect(10, 90, 200, 20), "Health: " + health, mystyle);
        GUI.Label(new Rect(10, 110, 200, 20), "Velocity Y: " + playerRB.linearVelocity.y, mystyle);
    }

}
