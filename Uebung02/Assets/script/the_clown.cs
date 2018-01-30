using UnityEngine;
using UnityEngine.UI;

public class the_clown : MonoBehaviour
{
    public Text text_velocity, text_points;
    public float start_velocity;
    public float m_velocity_multiplicator;
    public AudioClip m_wallsound, m_hitsound;

    private Rigidbody m_body;
    private AudioSource m_source;

    private float m_direction, m_velocity;
    private int m_points;


    // Use this for initialization
    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_source = GetComponent<AudioSource>();

        m_points = 0;
		m_direction = 0;

        PlaceClown(start_velocity);
        SetTexts();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.Translate(new Vector3(Mathf.Cos(m_direction) * m_velocity, 0, Mathf.Sin(m_direction) * m_velocity));
    }

    void OnMouseDown()
    {
		m_source.pitch = (1+ m_points/4);
        m_source.PlayOneShot(m_hitsound);
        ++m_points;

        PlaceClown(m_velocity * m_velocity_multiplicator);
        SetTexts();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            m_source.PlayOneShot(m_wallsound);
			m_direction += 90f;

			GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * 5, ForceMode.Acceleration);
            //m_body.velocity = Vector3.Reflect(m_body.velocity, collision.contacts[0].normal);

        }
    }

    void PlaceClown(float velocity)
    {
        transform.position = new Vector3(0f, .5f, 0f);
        m_direction = Random.Range(0f, 360f);
        m_velocity = velocity;
    }

    void SetTexts()
    {
        text_points.text = "Points: " + m_points;
        text_velocity.text = "Velocity: " + (m_velocity / start_velocity);
    }
}
