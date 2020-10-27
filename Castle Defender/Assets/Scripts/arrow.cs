using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    private float speed = 15f;
    private int arrowDamage = 20;
    private AudioSource source;
    public AudioClip shot;
    public AudioClip hitEnemy;
    public AudioClip hitStone;
    public AudioClip hitWood;

    void Start()
    {
        source = GameObject.Find("SoundSource").GetComponent<AudioSource>();
        SetArrowsDirectionAndRotation();
        source.PlayOneShot(shot);
    }

    void FixedUpdate()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "wood")
        {
            source.PlayOneShot(hitWood);
            col.gameObject.GetComponent<woodScript>().ReceiveDamage();
        }
        else if (col.gameObject.name.Contains("Enemy"))
        {
            source.PlayOneShot(hitEnemy);
            col.gameObject.GetComponent<EnemyLogic>().ReceiveDamage(arrowDamage);
        }
        else
        {
            source.PlayOneShot(hitStone);
        }
        Destroy(gameObject);
    }

    private void SetArrowsDirectionAndRotation()
    {
        if (gameObject.name == "PlayerArrow")
        {
            transform.position = GameObject.Find("PlayerBow").transform.position;
            transform.eulerAngles = GameObject.Find("PlayerBowDirection").transform.localEulerAngles;
        }
    }
}
