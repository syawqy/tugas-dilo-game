using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerControl player1;
    private Rigidbody2D player1RigidBody;

    public PlayerControl player2;
    private Rigidbody2D player2RigidBody;

    public BallControl ball;
    private Rigidbody2D ballRigidBody;
    private CircleCollider2D ballCollider;
    private bool isDebugWindow = false;
    public int maxScore;
    public Trajectory trajectory;
    // Start is called before the first frame update
    void Start()
    {
        player1RigidBody = player1.GetComponent<Rigidbody2D>();
        player2RigidBody = player2.GetComponent<Rigidbody2D>();
        ballRigidBody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2-150, 20, 100, 100), "" + player1.Score);
        GUI.Label(new Rect(Screen.width / 2+150, 20, 100, 100), "" + player2.Score);

        if(GUI.Button(new Rect(Screen.width/2 - 60,35,100,30), "RESTART"))
        {
            player1.ResetScore();
            player2.ResetScore();

            ball.SendMessage("RestartGame", 2, SendMessageOptions.RequireReceiver);
        }
        
        if(player1.Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");
            ball.SendMessage("ResetBall", 2, SendMessageOptions.RequireReceiver);
        }
        else if(player2.Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");
            ball.SendMessage("ResetBall", 2, SendMessageOptions.RequireReceiver);
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindow = !isDebugWindow;
            trajectory.ToggleActive(isDebugWindow);
        }

        if (isDebugWindow)
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            float ballMass = ballRigidBody.mass;
            Vector2 ballVelocity = ballRigidBody.velocity;
            float ballSpeed = ballRigidBody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            // Tentukan debug text-nya
            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);
            GUI.backgroundColor = oldColor;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
