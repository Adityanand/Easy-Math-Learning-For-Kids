using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamelogic : MonoBehaviour{
    public Camera ARCamera;
    //public GameObject UnityChan;
    public int a;
    public int b;
    public int ChanHealth;
    public int Health;
    public int result;
    public TextMesh A;
    public TextMesh B;
    public GameObject[] Results;
    public GameObject TryAgain;
    public Transform UnityChan;
    public Transform[] Destinations;
    public float Speed;

    // Use this for initialization
    void Start () {
        ChanHealth = 100;
        Health = 100;
        StartCoroutine(Add());
        Speed = 1f;
        StartCoroutine(UnityChanAI(UnityChan, Destinations, Speed));

    }
	
     IEnumerator Add()
    {
       a = Random.Range(1, 20);
       b = Random.Range(1, 20);
       result = a + b;
       A.text = a.ToString();
       B.text = b.ToString();  
       yield return null;
       StartCoroutine(ResultsValue());
    }
    IEnumerator ResultsValue()
    {
        UnityChan.GetComponent<Animator>().SetBool("Damage", false);
        UnityChan.GetComponent<Animator>().SetBool("Death", false);
        int i = Random.Range(0, Results.Length);
        var Result = Results[i];
        Result.GetComponentInChildren<TextMesh>().text = result.ToString();
        for (int j = 0; j < Results.Length; j++)
        {

            if (j != i)
            {
                Results[j].GetComponentInChildren<TextMesh>().text = Random.Range(10, 100).ToString();
            }
        }
        yield return null;
    }

    IEnumerator UnitychanDie()
    {
        yield return new WaitForSeconds(1.2f);
       Destroy(UnityChan);
       yield return new WaitForSeconds(1);
       SceneManager.LoadScene("Avidia New");
        
    }
    public void OnClickTryAgain()
    {
        SceneManager.LoadScene("Avidia New");
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    IEnumerator UnityChanAI(Transform target, Transform[] Destination, float speed)
    {
        foreach (var des in Destination)
        {
            if (Vector3.Distance(des.position, target.position) >= .01f)
            {
               target.position=Vector3.MoveTowards(target.position, des.position, speed);
                yield return new WaitForSeconds(2f);
            }
           
        }
        StartCoroutine(UnityChanAI(UnityChan, Destinations, Speed));
    }
    // Update is called once per frame
    void Update()
    {

      Ray ray = ARCamera.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit))
      {
        if (hit.collider.gameObject.tag=="Player")
        {
          if(Input.GetMouseButtonDown(0))
                {
                    if (result.ToString() == hit.collider.gameObject.GetComponentInChildren<TextMesh>().text)
                    {
                        ChanHealth -= 25;
                        UnityChan.GetComponent<Animator>().SetBool("Damage", true);
                        StartCoroutine(Add());
                    }
                    else
                    {
                        Health -= 25;
                        StartCoroutine(Add());
                    }
          }
        }
            if (ChanHealth == 0)
            {
               UnityChan.GetComponent<Animator>().SetBool("Death", true);
               StartCoroutine (UnitychanDie());
               
            }
            if(Health==0)
            {
                TryAgain.SetActive(true);
                for(int i=0;i<Results.Length;i++)
                {
                    Results[i].GetComponent<MeshCollider>().enabled = false;
                }
            }
      }
    }
}

