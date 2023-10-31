using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;
using JetBrains.Annotations;

public class Enemy : MonoBehaviour
{
    public int maxhp = 3;
    public int currenthp;
    public Animator ani;
    Transform target;
    public float speed = 5f;
    public bool ishit = false;
    //NavMeshAgent agent;
    public AudioClip dieSound;
    public GameObject peffect;

    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        ani = GetComponent<Animator>();
        currenthp = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Head");

        if (player != null)
        {
            target = player.transform;
        }

        if (currenthp <= 0)
        {
            StartCoroutine("animat");
            enabled = false;
        }

        else if(maxhp > 100 && currenthp <= 0)
        {
            pv.RPC("Kingdie", RpcTarget.AllBuffered);
        }

        if (player != null && target.position != transform.position)
        {
            gameObject.transform.LookAt(target);
            if (!ishit)
            {
                Vector3 direction = (target.position + new Vector3(0f, 0f, 2f)) - transform.position;
                direction.Normalize();

                transform.position += direction * speed * Time.deltaTime;
            }
            else if(ishit)
            {

            }
        }    
    }
 

    IEnumerator animat()
    {
        pv.RPC("dieani", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(1.5f);
        pv.RPC("die", RpcTarget.AllBuffered);
    }

    [PunRPC]

    void die()
    {
        Destroy(gameObject);
    }

    [PunRPC]

    void Kingdie()
    {
        GameManager.Instance.gameClearUI.SetActive(true);
    }

    [PunRPC]
    void dieani()
    {
        GetComponent<AudioSource>().PlayOneShot(dieSound);
        ani.SetTrigger("Death");
    }

    [PunRPC]
    void attackani()
    {
        ani.SetTrigger("Attack");
    }
    [PunRPC]
    
    public void hitani()
    {
        ani.SetTrigger("Hit");
    }

    [PunRPC]
    public void hurt(int hitdamage)
    {
        currenthp -= hitdamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Head")
        {
            Instantiate(peffect, gameObject.transform.position, Quaternion.identity);
            GameManager.Instance.kohit();
            other.GetComponentInParent<Player>().currenthp -= 1;
            pv.RPC("attackani",RpcTarget.All);
            FindObjectOfType<Cameramoving>().koRecoil();
        }
    }

    IEnumerator hitstate()
    {
        ishit = true;
        yield return new WaitForSeconds(0.5f);
        ishit = false;
    }

    [PunRPC]
    public void hithit()
    {
        StartCoroutine(hitstate());
    }


}
