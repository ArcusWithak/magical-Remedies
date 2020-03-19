using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    #region Properties - Player

    #region movement - variables
    public float health;
    public float maxHealth;

    public float movementSpeed { get; private set; }
    public float turnSpeed { get; private set; }

    public float speedIncrease;
    public float turnIncrease;

    public float turnSpeedIncrease { get; private set; }

    private float rotationY;
    private float vMove;
    #endregion

    #region ball - components
    private GameObject ballZ;
    private GameObject ballX;
    #endregion

    public List<GameObject> pickedUpItems;

    #endregion

    #region Constructor - Player

    public Player(float health, float movementSpeed, float turnSpeed, GameObject ballX)
    {
        this.health = health;
        maxHealth = health;

        this.movementSpeed = movementSpeed;
        this.turnSpeed = turnSpeed;

        speedIncrease = 1;
        turnIncrease = 1;

        this.ballX = ballX;
        ballZ = ballX.transform.GetChild(0).gameObject;

        pickedUpItems = new List<GameObject>(0);

        turnSpeedIncrease = 50;
    }

    #endregion

    #region Methods - Player

    #region SetFuctions - Player
    public void SetMovementSpeed(float value)
    {
        movementSpeed = value;
    }

    public void SetTurnSpeed(float value)
    {
        turnSpeed = value;
    }

    public void SetTurnSpeedIncrease(float value)
    {
        turnSpeedIncrease = value;
    }
    #endregion

    #region Movement - rotation - function
    public void MovementRotation(Transform user)
    {
        //movement
        rotationY = Input.GetAxis("Horizontal") * Time.deltaTime * (turnSpeed * turnIncrease);
        vMove = Input.GetAxis("Vertical") * Time.deltaTime * (movementSpeed * speedIncrease);

        user.Translate(0, 0, vMove);
        user.transform.position = new Vector3(Mathf.Clamp(user.transform.position.x, -125, 105), user.transform.position.y, Mathf.Clamp(user.transform.position.z, -135, 110));

        //rotation
        user.Rotate(0, rotationY, 0);

        //ball rolling
        if (vMove != 0)
        {
            ballX.transform.Rotate(vMove * turnSpeedIncrease, 0, 0, Space.Self);
        }

        if (rotationY != 0)
        {
            ballZ.transform.Rotate(0, 0, -rotationY, Space.Self);
        }
    }
    #endregion

    #region AddItemToBall - function
    public void AddItemToBall(GameObject other)
    {
        other.transform.SetParent(ballZ.transform);
        pickedUpItems.Add(other);
    }
    #endregion

    #endregion
}