using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public float xInitForce;
    public float yInitForce;

    private Rigidbody2D rigidbody2D;
    private Vector2 trajectoryOrigin;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //rigidbody2D.velocity.magnitude
    }

    void ResetBall()
    {
        transform.position = Vector2.zero;
        rigidbody2D.velocity = Vector2.zero;
    }

    void PushBall()
    {
        float yRandomForce = Random.Range(-yInitForce, yInitForce);
        float randomDir = Random.Range(0, 2);
        Vector2 vel;
        if(randomDir<1.0f)
        {
            vel = new Vector2(-xInitForce, yRandomForce);
        }else
        {
            vel = new Vector2(xInitForce, yRandomForce);
        }
        vel = vel.normalized;
        rigidbody2D.AddForce(vel*yInitForce);
    }

    void RestartGame()
    {
        ResetBall();
        Invoke("PushBall", 2);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}
