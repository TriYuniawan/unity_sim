using UnityEngine; // Mengimpor namespace Unity untuk mengakses komponen-komponen Unity
using UnityEngine.InputSystem; // Mengimpor namespace Input System baru dari Unity untuk menangani input
using UnityEngine.UIElements; // Mengimpor namespace UI Toolkit untuk mengakses UIDocument dan elemen UI lainnya

public class PlayerController : MonoBehaviour // Mendefinisikan kelas PlayerController yang mewarisi MonoBehaviour agar bisa digunakan sebagai komponen Unity
{
    private float elapsedTime = 0f; // Variabel untuk menyimpan total waktu yang telah berjalan sejak game dimulai
    public float thrustForce = 10f; // Kekuatan dorongan yang diberikan pada objek saat tombol mouse ditekan, dapat diubah di Inspector
    public float maxSpeed = 10f; // Kecepatan maksimum yang diizinkan untuk objek, dapat diubah di Inspector
    Rigidbody2D rb; // Variabel untuk menyimpan referensi komponen Rigidbody2D yang ada pada objek ini
    private float Score = 0f;
    public float scoreMultiplier = 10f;
    public UIDocument uiDocument;

    void Start() // Fungsi Start dipanggil sekali saat objek pertama kali diaktifkan
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari objek ini dan menyimpannya ke variabel rb

        if (rb == null) // Mengecek apakah komponen Rigidbody2D berhasil ditemukan atau tidak
            Debug.LogError("Rigidbody2D tidak dijumpai pada " + gameObject.name); // Menampilkan pesan error di console jika Rigidbody2D tidak ditemukan
    }

    void Update() // Fungsi Update dipanggil setiap frame selama game berjalan
    {
        elapsedTime += Time.deltaTime; // Menambahkan waktu antar frame ke elapsedTime agar mencerminkan total waktu berjalan

        Score =  Mathf.Floor(elapsedTime * scoreMultiplier); // menghitung skor berdasarakan waktu yang telah berjalan


        Debug.Log("Elapsed Time : " + elapsedTime); // Menampilkan nilai elapsedTime di console Unity setiap frame
        Debug.Log("Score : " + Score); // Menampilkan nilai Score di console Unity setiap frame

        if (Mouse.current.leftButton.isPressed) // Mengecek apakah tombol kiri mouse sedang ditekan saat ini
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // Mengkonversi posisi kursor mouse dari koordinat layar ke koordinat dunia (world space)
            mousePos.z = transform.position.z; // Menyamakan nilai Z mouse dengan Z objek agar tidak ada perbedaan kedalaman pada 2D

            Vector2 direction = (mousePos - transform.position).normalized; // Menghitung arah dari posisi objek menuju posisi mouse, lalu dinormalisasi menjadi vektor satuan

            transform.up = direction; // Memutar objek agar bagian atas (up) menghadap ke arah mouse
            rb.AddForce(direction * thrustForce, ForceMode2D.Force); // Menambahkan gaya fisika ke Rigidbody2D ke arah mouse dengan besar thrustForce
        }

        if (rb.linearVelocity.magnitude > maxSpeed) // Mengecek apakah kecepatan saat ini melebihi batas maxSpeed
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed; // Membatasi kecepatan objek agar tidak melebihi maxSpeed dengan mempertahankan arahnya
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // Fungsi yang otomatis dipanggil saat objek ini bertabrakan dengan objek lain yang memiliki Collider2D
    {
        Destroy(gameObject); // Menghancurkan (menghapus dari scene) objek ini saat terjadi tabrakan
    }
}