using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum MoveType
    {
        center,up,right,circle
    }
    private MoveType moveType;
    private int randomMove;
    public float speed = 1f;
    public int EnemyHealth;
    public int currentEnemyHealth;
    [SerializeField] float range;
    Vector3 dir;
    GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
       randomMove = Random.Range(0, 3); // need to be up to 4 for circle
       currentEnemyHealth = EnemyHealth;
       gm = GameObject.FindGameObjectWithTag("GameManager");
       dir = gameObject.transform.position - new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movemnt();
         Collider2D[] borders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x,transform.position.y), range);
        foreach(Collider2D col in borders)
        {
           if(col.gameObject.layer==9)
            {
                gm.GetComponent<GameManager>().health--;
                gm.GetComponent<GameManager>().birdCount--;
                gm.GetComponent<GameManager>().healthText.text = "Health: " + gm.GetComponent<GameManager>().health;
                Destroy(gameObject);
            }
            
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(transform.position,range);
    }

    private void Movemnt()
    {
        dir = dir.normalized;
        switch(randomMove)
        {
            
            case 0: transform.position -= dir.normalized * speed * Time.deltaTime; break;
            case 1: transform.position -= new Vector3(0,dir.y,0) * speed * Time.deltaTime; break;
            case 2: transform.position -= new Vector3(dir.x,0, 0) * speed * Time.deltaTime; break;
            case 3: break;
        }
       
    }

}
