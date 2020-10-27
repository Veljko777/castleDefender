using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private bool playerFiring = false;
    public bool startFire;

    public bool playerWalking = false;
    private float fireSpeed = 0.25f;
    private float playerMaxHp = 100;
    private float playerCurrentHp = 100;
    private GameObject hpBar;
    public GameObject arrow;

    public AudioClip gettingHit;
    private AudioSource sound;

    private Animator anim;
    void Start()
    {
        startFire = false;
        sound = GameObject.Find("SoundSource").GetComponent<AudioSource>();
        hpBar = GameObject.Find("playerHPBar");
        anim = transform.Find("PlayerImage").GetComponent<Animator>();
        SetHPBar();
    }
    void Update()
    {
        if (!GameLogic.isPlayerDead)
        {
            if (startFire || Input.GetMouseButton(1))
            {
                Fire();
            }
            if (playerWalking)
            {
                
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }
        }
    }

    public void Fire()
    {
        if (!playerFiring)
        {
            StartCoroutine(FireArrow());
        }
    }
    private void SetHPBar()
    {
        hpBar.transform.localScale = new Vector3(playerCurrentHp/playerMaxHp, hpBar.transform.localScale.y);
        Color32 barColor;
        if (playerCurrentHp < 1)
        {
            barColor = new Color32(255, 255, 255, 0);
        }
        else if (playerCurrentHp <= 20)
        {
            barColor = new Color32(255, 0, 0, 255);
        }
        else if (playerCurrentHp <= 50)
        {
            barColor = new Color32(255, 150, 0, 255);
        }
        else
        {
            barColor = new Color32(0, 255, 0, 255);
        }
        hpBar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = barColor;
    }

    public void GetHit(int damage)
    {
        if (damage >= playerCurrentHp)
        {
            GameLogic.isPlayerDead = true;
            anim.SetBool("Attack", false);
            anim.SetBool("Walking", false);
            anim.SetTrigger("Die");
            Destroy(transform.GetComponent<Rigidbody2D>());
            Destroy(transform.GetComponent<CapsuleCollider2D>());
        }
        else
        {
            anim.SetTrigger("GetHit");
        }
        sound.PlayOneShot(gettingHit);
        playerCurrentHp -= damage;
        SetHPBar();
    }

    IEnumerator CreateArrow()
    {
        yield return new WaitForSeconds(fireSpeed / 2);
        GameObject go = Instantiate(arrow);
        go.name = "PlayerArrow";
    }
    public IEnumerator FireArrow()
    {
        anim.SetBool("Attack", true);
        playerFiring = true;
        StartCoroutine(CreateArrow());
        yield return new WaitForSeconds(fireSpeed);
        playerFiring = false;
        anim.SetBool("Attack", false);
    }


}
