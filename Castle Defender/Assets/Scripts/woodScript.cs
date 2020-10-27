using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodScript : MonoBehaviour
{
    public Sprite damagedWood;
    public Sprite moreDamagedWood;
    public Sprite dirt;
    public int TileLife=3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveDamage()
    {
        TileLife--;
        SetTileStatus();
    }
    public void SetTileStatus()
    {
        switch (TileLife)
        {
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = damagedWood;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = moreDamagedWood;
                break;
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = dirt;
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                gameObject.layer = 0;
                break;
        }
    }
}
