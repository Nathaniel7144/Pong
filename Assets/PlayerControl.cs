using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    /**
    * Deklarasi variable yang dibutuhkan.
    */

    //Tombol untuk menggerakan racket ke atas
    public KeyCode upButton = KeyCode.W;

    //Tombol untuk menggerakan racket ke bawah
    public KeyCode downButton = KeyCode.S;

    //Kecepatan gerak
    public float speed = 10.0f;

    //Batas atas dan bawah game scene (Batas bawah menggunakan minus (-))
    public float yBoundary = 9.0f;

    //Variable status powerup
    public bool isPoweredUp;

    //Rigidbody 2D racket
    private Rigidbody2D rigidBody2D;

    //Variable ball untuk ambil value OnFire atau tidak
    public BallControl ball;

    //Variable gameManager untuk akses max score
    public GameManager gameManager;

    //Variable untuk lawan
    public PlayerControl opponent;

    //Score
    private int score;

    //Variable untuk debug
    //Titik tumbukan terakhir dengan bola, untuk menampikan variable-variable fisika terkait tumbukan tersebut
    private ContactPoint2D lastContactPoint;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name.Equals("Ball"))
        {
            lastContactPoint = collision.GetContact(0);

            //Ketika bertumbukan dengan bola setelah 2 detik,
            //Jika bola belum menjadi bola api 
            //Invoke rollFireBall menggunakan CallRollFireBall
            if(ball.IsBallOnFire == false)
            {
                ball.CallRollFireBall(1.0f);
            }
            //Jika bola merupakan bola api dan mengenai player
            else
            {
                //Jika player tidak powerup
                if (isPoweredUp == false)
                {
                    //lawan bertambah scorenya
                    opponent.IncrementScore();

                    //Jika score belum mencapai maksimal
                    if(Score < gameManager.maxScore)
                    {
                    //...restart game setelah bola api mengenai player
                    collision.gameObject.SendMessage("RestartGame",
                        2.0f, SendMessageOptions.RequireReceiver);
                    }
                } 
                else 
                {
                    //Jika player poweredup dan berhasil memantulkan bola
                    ball.CallRollFireBall(1.0f);
                }
                
            }

        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get kecepatan racket sekarang
        Vector2 velocity = rigidBody2D.velocity;

        //Jika pemain menekan tombol ke atas, beri kecepatan positif ke komponen sumbu y
        if(Input.GetKey(upButton))
        {
            velocity.y = speed;
        }

        //Jika pemain menekan tombol ke bawah, beri kecepatan negatif ke komponen sumbu y
        else if(Input.GetKey(downButton))
        {
            velocity.y = -speed;
        }

        //Jika pemain tidak menekan tombol apa2, kecepatannya nol.
        else
        {
            velocity.y = 0.0f;
        }

        //Masukkan kembali kecepatannya ke rigidBody2D
        rigidBody2D.velocity = velocity;

        //Get posisi racket sekarang
        Vector3 position = transform.position;

        //Jika posisi racket melewati batas atas (yBoundary), kembalikan ke batas atas tersebut
        if(position.y > yBoundary)
        {
            position.y = yBoundary;
        }

        //Jika posisi racket melewati batas bawah (-yBoundary), kembalikan ke batas atas tersebut
        else if(position.y < -yBoundary)
        {
            position.y = -yBoundary;
        }

        //Masukkan kembali posisinya ke transform
        transform.position = position;
    }

    public void IncrementScore()
    {
        score++;
    }

    public void ResetScore()
    {
        score = 0;
    }

    //Property untuk mengakses variable score dari class lain
    public int Score
    {
        get { return score; }
    }

    //Property untuk akses variable lastContactPoint
    public ContactPoint2D LastContactPoint
    {
        get { return lastContactPoint; }
    }

    //Property untuk akses status powerup
    public bool IsPoweredUp
    {
        get { return isPoweredUp; }
    }

    //Dapatkan powerup untuk Player
    //Ukuran racket akan bertambah panjang selama 8 detik
    public IEnumerator getPowerUp()
    {
        Vector3 originalScale = transform.localScale;

        transform.localScale = new Vector3(1, 2, 1);

        yield return new WaitForSeconds(10f);
        //setelah 10 detik

        transform.localScale = originalScale;

        isPoweredUp = false;
    }
}
