using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{

    //Declare gameManager untuk meangkses score maksimal
    [SerializeField]
    private GameManager gameManager = null;

    //Pemain yang akan bertambah skornya jika bola menyentuh dinding ini
    public PlayerControl player;

    public PowerUp powerup;

    //Akan dipanggil ketika objek lain (bola) collide 
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        //Jika objek tersebut bernama "Ball"
        if(anotherCollider.name == "Ball")
        {
            //tambahkan score
            player.IncrementScore();
            
            //Jika score belum mencapai maksimal
            if(player.Score < gameManager.maxScore)
            {
                //...restart game setelah bola mengenai dinding
                anotherCollider.gameObject.SendMessage("RestartGame",
                    2.0f, SendMessageOptions.RequireReceiver);
            }
        }
        //Jika powerup bertabrakan dengan PowerUp, maka reset posisi kemudian launch powerup
        else if (anotherCollider.name == "PowerUp")
        {
            powerup.resetPow();
            powerup.launch();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
