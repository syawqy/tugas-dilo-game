using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public PlayerControl player;

    [SerializeField]
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Ball")
        {
            player.IncrementScore();
            
            if(player.Score < gameManager.maxScore)
            {
                collision.gameObject.SendMessage("RestartGame", 1, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
