
using UnityEngine;

public class BallControl : MonoBehaviour
{

    //Rigidbody2D bola
    private Rigidbody2D rigidBody2D;

    //Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;

    //Variable untuk debug
    //Titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;

    //Variable yang menentukan bola api/ tidak
    private bool isBallOnFire = false;

    public bool IsBallOnFire
    {
        get { return isBallOnFire; }
        set { isBallOnFire = value; }
    }

    /**
    * Method untuk melakukan reset posisi dan gaya pada bola
    */
    void ResetBall()
    {
        //reset posisi menjadi (0,0)
        transform.position = Vector2.zero;

        //Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;

        IsBallOnFire = false;
    }

    //Method untuk Invoke (delay)
    public void CallRollFireBall(float time)
    {
        Invoke("rollFireBall", time);
    }

    //Method untuk menentukan Fireball/ tidak ketika bola direset
    private void rollFireBall()
    {
        //tentukan angka random untuk membuat bola api
        // 0, 1
        float randomFireBall = Random.Range(0, 2);

        //Jika angka == 1 maka ubah jadi fireball
        if (randomFireBall == 1)
        {
            IsBallOnFire = true;
        } else 
        {
            IsBallOnFire = false;
        }
    }

    /**
    * Method untuk melakukan inisialisasi gerakan bola
    */
    void PushBall()
    {
        //Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        //float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);

        //Tentukan nilai acak antara 0 (inclusive) dan 2 (exclusive)
        float randomDirection = Random.Range(0, 2);

        //Jika nilainya di bawah 1, bola bergerak ke kiri
        //Jika tidak, bola bergerak ke kanan
        if(randomDirection < 1.0f)
        {
            //Gunakan gaya untuk menggerakan bola ini.
            rigidBody2D.AddForce(new Vector2(-xInitialForce, yInitialForce));
        }
        else 
        {
            rigidBody2D.AddForce(new Vector2(xInitialForce, yInitialForce));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    /**
    * Method yang akan dipanggil setiap kali game diulang, pertama mulai, atau lewat racket
    */
    void RestartGame()
    {
        //Kembalikan bola ke posisi semula
        ResetBall();

        //Setelah 2 detik, berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();

        //Mulai Game
        Invoke("PushBall", 2);

        trajectoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Property untuk akses class trajectory origin dari class lain
    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }

}
