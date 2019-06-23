using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGame2_Block : MonoBehaviour
{
    private bool boclEnabled = true;
    private GameObject particleGo;

    public GameObject dustParticles;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (boclEnabled)
        {
            if (collision.contacts.Length > 0)
            {
                boclEnabled = false;
                Vector3 v = gameObject.transform.position;
                v.y = v.y - 1.0f;
                dustParticles.transform.position = v;
                particleGo = Instantiate(dustParticles, v, Quaternion.identity);
                StartCoroutine(Deactivate());
            }
        }
    }

    IEnumerator  Deactivate()
    {
        yield return new WaitForSeconds(1);

        Destroy(particleGo, 1);
    }
}
