
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    /**
    * Deklarasi variable yang diperlukan
    */

    //Rigidbody dari powerup
    private Rigidbody2D rigidBody2D;

    //Posisi awal akan diberikan gaya
    //X = 0 agar selalu mulai dari tengah
    public static float initialPosX = 0;
    public float initialPosY;

    //Besarnya gaya awal yang diberikan untuk mendorong bola
    //Gaya tidak berubah
    public float xInitialForce;
    public static float yInitialForce = 10;

    //Variable Player
    public PlayerControl player1;
    public PlayerControl player2;

    //Method untuk beri gaya pada powerup
    void PushPow()
    {
        Vector2 velocity = rigidBody2D.velocity;

        //random arah
        float randomDirection = Random.Range(0, 2);

        if(randomDirection < 1.0f)
        {
            //Gunakan gaya untuk menggerakan bola ini pertama kali.
            rigidBody2D.AddForce(new Vector2(-xInitialForce, yInitialForce));

            //Berikan kecepatan sehingga bola dapat bergerak horizontal
            velocity.x = -6f;
        }
        else 
        {
            //Gunakan gaya untuk menggerakan bola ini pertama kali.
            rigidBody2D.AddForce(new Vector2(xInitialForce, yInitialForce));

            //Berikan kecepatan sehingga bola dapat bergerak horizontal
            velocity.x = 6f;
        }

        rigidBody2D.velocity = velocity;
    }

    //Lakukan push terhadap Powerup setelah 5 detik
    public void launch()
    {
        Invoke("PushPow", 4);
    }

    //Ubah posisi awal spawn dari powerup
    public void resetPow()
    {
        //random posisi Y
        float randomPositionY = Random.Range(-6, 7);

        //ubah posisi menjadi posisi y (random), dan x tetap 0 sehingga posisi tetap dari tengah
        transform.position = new Vector2(initialPosX, randomPositionY);

        //ubah gaya = 0
        rigidBody2D.velocity = Vector2.zero;
    }

    //Ketika bertabrakan dengan racket (player), maka Powerup akan direset
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        //Jika objek tersebut bernama "Player1"
        if(anotherCollider.name == "Player1")
        {
            if(player1.isPoweredUp == false)
            {
                player1.isPoweredUp = true;

                //ubah ukuran racket
                StartCoroutine(player1.getPowerUp());
                
                resetPow();

                launch();
            }
        } else if(anotherCollider.name == "Player2")
        {
            if(player2.isPoweredUp == false)
            {
                player2.isPoweredUp = true;

                //ubah ukuran racket
                StartCoroutine(player2.getPowerUp());
                
                resetPow();

                launch();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        rigidBody2D = GetComponent<Rigidbody2D>();

        //Mulai berikan power up
        launch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
