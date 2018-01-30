using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodScript : MonoBehaviour
{
    public Text m_text;
    public AudioClip m_pipesound;
    public GameObject Platform, Player, PowerUP;
    public int powerCounter;

    private AudioSource m_audio;
    private List<GameObject> powerUps;
    private GameObject lastPlatform, player;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        powerUps = new List<GameObject>();
        powerCounter = 0;

        player = Instantiate(Player, new Vector3(0f, 5f, 0f), Quaternion.Euler(0, 0, 0));

        lastPlatform = Instantiate(Platform, new Vector3(0f, 0f, 0f), Quaternion.Euler(0, 0, 0));
        lastPlatform.transform.localScale = new Vector3(4f, 1f, 1f);
    }

    void Update()
    {
        // is Player Alive?
        if (player.transform.position.y < -5)
        {
            m_audio.PlayOneShot(m_pipesound);

            // reset position
            player.transform.position = new Vector3(0f, 8f, 0f);
            // empty Power Ups
            powerUps.Clear();
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }


        // create Platform and PowerUp
        float platformX = lastPlatform.transform.position.x;
        float cameraX = transform.position.x;

        // remove all dead powerups
        powerUps.RemoveAll(item => item == null);

        // count how many PowerUps the Player collected
        int curpower = powerUps.FindAll(item => item.GetComponent<PowerUps>().isDying).Count;

        // create Platforms offscreen

        // move Camera
        if (curpower != powerCounter)
        {
            powerCounter = curpower;
            m_audio.pitch = 1f + powerCounter * .03f;
        }

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, -(powerCounter * 2.5f + 16f));
        transform.position = Vector3.MoveTowards(transform.position, target, 0.3f);


        if (cameraX + (-transform.position.z) >= platformX)
        {

            float offset = platformX + powerCounter * 2.5f + 6f;
            float height = Random.value * 4f;

            // create new Platform
            GameObject newPlatform = Instantiate(
                Platform,
                new Vector3(offset, height, 0f),
                Quaternion.Euler(0, 0, 0));

            // width of platform
            newPlatform.transform.localScale = new Vector3(Random.value * 4f + 2f, 1f, 1f);
            lastPlatform = newPlatform;

            // maybe create new Powerup at the platform
            if (Random.value > .6f)
            {
                GameObject p = Instantiate(
                    PowerUP,
                    lastPlatform.transform.position + Vector3.up * 3f,
                    Quaternion.Euler(0, 0, 0));
                powerUps.Add(p);
            }
        }
        m_text.text = "Power Ups: " + powerCounter;
    }
}