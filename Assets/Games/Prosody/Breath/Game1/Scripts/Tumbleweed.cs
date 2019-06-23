using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweed : MonoBehaviour {
    public Game game;
    public GameObject dustParticles;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            if (collision.contacts.Length > 0)
            {
                Vector3 v = collision.contacts[0].point;
                dustParticles.transform.position = v;
                GameObject go = Instantiate(dustParticles, v, Quaternion.identity);
                Destroy(go, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Finish"))
        {
            game.FinishGame();
        }
    }
}
