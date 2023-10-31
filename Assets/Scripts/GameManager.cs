using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public bool isgameover = true;
    static public float time;
    static public float bosstime;
    static public float playtime = 180f;
    float spawntime;
    float maxtime = 1.5f;
    float mintime = 0.8f;
    int bosscount = 0;
    int minute;
    int second;

    public Text timetext;
    public GameObject panel;

    public GameObject gun1;
    public GameObject gun2;
    public GameObject leftcrosshair;
    public GameObject rightcrosshair;

    public GameObject gameOverUI;
    public GameObject gameClearUI;

    public PhotonView pv;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spawntime = Random.Range(mintime, maxtime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isgameover)
        {
            pv.RPC("Minustime", RpcTarget.AllBuffered);
            minute = (int)playtime % 3600 / 60;
            second = (int)playtime % 3600 % 60;
            timetext.text = minute + ":" + second;
            timetext.gameObject.SetActive(true);

            gun1.SetActive(true);
            gun2.SetActive(true);
            leftcrosshair.SetActive(true);
            rightcrosshair.SetActive(true);

            time += Time.deltaTime;
            bosstime += Time.deltaTime;
        }
        else if (isgameover)
        {
            gun1.SetActive (false);
            gun2.SetActive (false);
            leftcrosshair.SetActive(false);
            rightcrosshair.SetActive(false);

            timetext.gameObject.SetActive(false);
        }

        if (!isgameover && bosstime < 120f)
        {
            if (time > spawntime)
            {
                createcrab();
                time = 0f;
            }
        }
        else if (!isgameover && bosstime >= 120f && bosscount == 0)
        {
            if (bosscount<PhotonNetwork.PlayerList.Length)
            {
                createkingcrab();
                ++bosscount;
            }
        }
        
    }

    [PunRPC]

    void Minustime()
    {
        playtime -= Time.deltaTime;

        if (playtime < 0f) {playtime = 0f;}
    }

    public void createcrab()
    {
        Vector3 vec = new Vector3(Random.Range(-73f, 73f), 10f, Random.Range(50f, 90f));

        PhotonNetwork.Instantiate("Enemy", vec, Quaternion.Euler(0f,180f,0f));
       
    }

    public void createkingcrab()
    {
        Vector3 kvec = new Vector3(0f, 10f, 88f);
        PhotonNetwork.Instantiate("BossEnemy", kvec, Quaternion.Euler(0f, 180f, 0f));
    }

    IEnumerator hitpanel()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        panel.SetActive(false);
    }

    public void kohit()
    {
        StartCoroutine("hitpanel");
    }
}
