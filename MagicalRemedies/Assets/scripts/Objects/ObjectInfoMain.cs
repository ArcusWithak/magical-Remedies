using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfoMain : MonoBehaviour
{
    #region properties

    [Space]
    [Header("the increase in Score when you pick up the item")]
    [SerializeField]
    [Space(20)]
    public int score;

    [Space]
    [Header("how much weight does picking up item give")]
    [SerializeField]
    [Space(20)]
    public float weight;

    [Space]
    [Header("Is the Item a Component or a random pickup?")]
    [SerializeField]
    [Space(20)]
    public bool nessasryComponent;

    #region different potions
    public enum PotionState
    {
        none = 0,
        damage = 1,
        healing = 2,
        speed = 3
    }

    [Space]
    [Header("does item have special property?")]
    [Space(20)]
    public PotionState itemstate;

    [Space]
    [Header("damage, healing, duration amount based on potion state")]
    [Space(20)]
    public float potionValue;
    #endregion

    private Light objectLight;

    private float speed;
    private float yStart;
    private bool blockmovement = true;

    #endregion

    #region Methods

    protected virtual void Start()
    {
        speed = 0.5f;

        yStart = transform.position.y;

        gameObject.tag = "GrabbleObject";

        if(weight == GameManager.instance.currentLevel)
        {
            nessasryComponent = true;
        }
        else
        {
            nessasryComponent = false;
        }
        //is object special potion
        if (itemstate != PotionState.none)
        {
            blockmovement = false;
            nessasryComponent = false;

            #region addLight
            //see if light components is present
            objectLight = GetComponent<Light>();
            if (objectLight == null)
            {
                //add if light is missing
                objectLight = gameObject.AddComponent<Light>();
            }

            //set light to correct color and brightness
            objectLight.range = 2;
            objectLight.intensity = 2f;
            #endregion

            #region action based on potion type
            //if damaging potion
            if (itemstate == PotionState.damage)
            {
                objectLight.color = new Color(150, 0, 255);
            }
            //if healing potion
            else if (itemstate == PotionState.healing)
            {
                objectLight.color = new Color(255, 0, 0);
            }
            //if speed potion
            else if (itemstate == PotionState.speed)
            {
                objectLight.color = new Color(255, 210, 0);
            }
            #endregion
        }
    }

    public void startPotionTimer(Player stat)
    {
        StartCoroutine(PotionTimer(stat));
    }

    public IEnumerator PotionTimer(Player stats)
    {
        blockmovement = true;
        stats.speedIncrease = 2;
        stats.turnIncrease = 2;
        yield return new WaitForSeconds(potionValue);
        stats.speedIncrease = 1;
        stats.turnIncrease = 1;
        Destroy(this);
    }

    protected virtual void Update()
    {
        if (!blockmovement)
        {
            if (transform.position.y < yStart - 0.1f || transform.position.y > yStart + 0.1f) { speed = -speed; }
            transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
        }
    }
    #endregion
}
