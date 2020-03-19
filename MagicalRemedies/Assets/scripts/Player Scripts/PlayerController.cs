using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region movement - variables
    [Space]
    [Header("speed at which you move")]
    [Space(10)]
    public float movementSpeed;

    [Space]
    [Header("speed you can turn at")]
    [Space(10)]
    public float turnSpeed;
    #endregion

    #region refrencese
    public Player stats;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        stats = new Player(50, movementSpeed, turnSpeed, transform.GetChild(1).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        stats.MovementRotation(transform);
    }

    //check if item is picked up
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GrabbleObject"))  
        {   
            //set variables on new item
            GameObject otherObject = other.gameObject;
            Object script = other.gameObject.GetComponent<Object>();
            if (script.weight <= GameManager.instance.currentLevel)
            {
                //add item to list
                stats.AddItemToBall(other.gameObject);

                //increase score
                GameManager.instance.SetScore(script.score, true);

                //is object special potion
                if (script.itemstate != ObjectInfoMain.PotionState.none)
                {
                    #region action based on potion type
                    //if damaging potion
                    if (script.itemstate == ObjectInfoMain.PotionState.damage)
                    {
                        GameManager.instance.damageLines[Random.Range(0, GameManager.instance.damageLines.Count)].Play();
                        if (stats.health > 0)
                        {
                            stats.health -= script.potionValue;
                        }
                        Destroy(other.gameObject);
                    }
                    //if healing potion
                    else if (script.itemstate == ObjectInfoMain.PotionState.healing)
                    {
                        stats.health += script.potionValue;
                        if (stats.health > stats.maxHealth)
                        {
                            stats.health = stats.maxHealth;
                        }
                        Destroy(other.gameObject);
                    }
                    //if speed potion
                    else if (script.itemstate == ObjectInfoMain.PotionState.speed)
                    {
                        script.startPotionTimer(stats);
                        GameManager.instance.SpeedUp.Play();
                    }
                    #endregion
                }
                //removes extra components
                if (otherObject.GetComponent<Light>()) { Destroy(otherObject.GetComponent<Light>()); }
                if (otherObject.GetComponent<Rigidbody>()) { Destroy(otherObject.GetComponent<Rigidbody>()); }
                MeshCollider oldCollider = other.gameObject.GetComponent<MeshCollider>();
                if(script.weight >= 4 && oldCollider)
                {
                    Destroy(oldCollider);

                    SphereCollider newCollider = other.gameObject.AddComponent<SphereCollider>();

                    newCollider.center = new Vector3(-0.004564872f, -0.006794827f, 0);
                    newCollider.radius = 0.05882943f;
                }

                //increase score
                if (script.nessasryComponent) { GameManager.instance.IncreaseItemCount(); }

                //change tag to ball
                other.gameObject.tag = "Ball";
            }
        }
    }


    #region EmptyBall - function
    public void ClearBall()
    {
        for (int i = 0; i < stats.pickedUpItems.Count; i++)
        {
            Destroy(stats.pickedUpItems[i]);
            stats.pickedUpItems.Remove(stats.pickedUpItems[i]);
        }
    }
    #endregion
}
