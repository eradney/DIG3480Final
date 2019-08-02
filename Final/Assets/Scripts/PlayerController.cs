using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public float fireRate;
    public float boost;
    public Boundary boundary;
    public GameObject shot;
    public Transform shotSpawn;
    public AudioSource MusicSource;
    public AudioSource pickupSource;
    public AudioClip PlayerWeapon;
    public AudioClip pickupClip;

    private float nextFire;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            MusicSource.clip = PlayerWeapon;
            MusicSource.Play();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;
        rb.position = new Vector3
        (
             Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
             0.0f,
             Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            speed = speed * boost;
            other.gameObject.SetActive(false);
            StartCoroutine("waitTime");
            pickupSource.clip = pickupClip;
            pickupSource.Play();
        }
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(3);
        speed = speed / boost;
    }
}
