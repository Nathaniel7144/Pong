using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Implementasi UI pada game
*/

public class GameManager : MonoBehaviour
{

    //Pemain 1
    public PlayerControl player1; //script
    private Rigidbody2D playerRigidbody;

    //Pemain 2
    public PlayerControl player2; //script
    private Rigidbody2D player2Rigidbody;

    //Bola
    public BallControl ball; //script
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;

    //score maksimal
    public int maxScore;

    //PowerUp
    public PowerUp pow; //script
    private Rigidbody2D powRigidbody;
    private CircleCollider2D powCollider;
    //GameObject untuk SetActivate pertama kali akan diluncurkan
    public GameObject powObj;

    /**
    *Variable untuk debug
    */
    //Apakah debug window ditampilkan?
    private bool isDebugWindowShown = false;

    //Variable untuk menampilkan trajectory
    //Objek untuk prediksi lintasan bola
    public Trajectory trajectory;

    //Karena GameObject di-disabled, maka perlu diactivate kembali
    //Agar ada jeda waktu aktivasi, dibuat method yang akan diInvoke nanti
    void ActivatePow()
    {
        powObj.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Inisialisasi rigidbody dan collider
        playerRigidbody = player1.GetComponent<Rigidbody2D>();
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        powRigidbody = pow.GetComponent<Rigidbody2D>();
        powCollider = pow.GetComponent<CircleCollider2D>();

        //Beri jeda waktu untuk aktivasi GameObject
        Invoke("ActivatePow", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Untuk menampilkan GUI
    void OnGUI()
    {
        //Tampilkan score pemain 1 di kiri atas dan pemain 2 di kanan atas
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);

        
        //Toggle nilai debug window ketika pemain klik tombol ini.
        if(GUI.Button(new Rect(Screen.width/2 - 60, Screen.height - 73, 120, 53), "TOGGLE \nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;
        }

        //Tombol restart untuk memulai game dari awal
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 52), "RESTART"))
        {
            //Ketika tombol restart ditekan, reset score kedua pemain...
            player1.ResetScore();
            player2.ResetScore();

            pow.resetPow();

            //... kemudian restart game
            ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        //Jika pemain 1 menang (mencapai score maksimal)
        if(player1.Score == maxScore)
        {
            //tampilkan teks "PLAYER ONE WINS" di bagian kiri layar...
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");

            pow.resetPow();

            //dan kembalian bola ke tengah
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        //Sebalikya jika pmeain 2 menang (mencapai score maksimal)
        else if(player2.Score == maxScore)
        {
            //tampilkan teks "PLAYER TWO WINS" di bagian kiri layar...
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");

            pow.resetPow();

            //dan kembalian bola ke tengah
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        //Jika isDebugWindowShown == true, tampilkan text area untuk debug window
        if(isDebugWindowShown)
        {
            //Simpan nilai warna lama GUI
            Color oldColor = GUI.backgroundColor;

            //Beri warna baru
            GUI.backgroundColor = Color.red;

            /**
            *Variable untuk debugWindow, nilai-nilai variable fisika
            */
            //massa bola, diakses melalui rigidBody2D menjelaskan resistensi objek terhadap perubahan gerakan
            float ballMass = ballRigidbody.mass;
            
            //kecepatan, vector yang mendeskripsikan perubahan posisi objek terhadap waktu
            Vector2 ballVelocity = ballRigidbody.velocity;
            
            //laju, merupakan besarnya kecepatan
            float ballSpeed = ballRigidbody.velocity.magnitude;
            
            //Momentum, menjelaskan seberapa susahnya menghentikan sebuah objek dari gerakan semulanya. Semakin besar massa dan kecepatan.
            //makin susah benda tersebut dihentikan.
            Vector2 ballMomentum = ballMass * ballVelocity;

            //Friction, gaya gesek yang diterapkan pada objek terhadap objek lain
            float ballFriction = ballCollider.friction;

            //Impulse, total gaya yang diterapkan kepada sebuah objek selama rentang waktu tertentu.
            //Pada game engine umumnya impluse adalah gaya instan dengan nilai besar yang diterapkan kepada sebuah objek
            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            //Tentukan debug text
            string debugText = 
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n" +
                "Player1 PowerUp " + player1.IsPoweredUp + " \n" +
                "Player2 PowerUp " + player2.IsPoweredUp + " \n";

            //Tampilkan debugWindow
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 140), debugText, guiStyle);

            //Kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }

        //Jika ball on fire, maka tampilkan "THE BALL IS ON FIRE!"
        if(ball.IsBallOnFire)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + -150, 500, 1000), "THE BALL IS ON FIRE!");
        }
    }
}
