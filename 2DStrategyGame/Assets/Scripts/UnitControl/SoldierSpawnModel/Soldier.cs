using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    #region States
    private enum State
    { 
        Idle,
        Walk,
        Attack,
    }

    private State currentState;
    private bool isWalking;
    private bool isAttacking;

    #endregion

    #region Soldier Base
    private const float MOVE_SPEED = 20F;
    public int Health { get; set; }
    public int SoldierID { get; set; }
    public int Damage { get; set; }

    #endregion

    private void Start()
    {
        currentState = State.Idle;
    }

    public void Initialize(int health, int id, int damage)
    {
        Health = health;
        SoldierID = id;
        Damage = damage;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:

                if (isWalking) currentState = State.Walk;

                break;
            case State.Walk:

                if (!isWalking && isAttacking) currentState = State.Attack;
                else if (!isWalking && !isAttacking) currentState = State.Idle;

                break;
            case State.Attack:

                if (isWalking) currentState = State.Walk;

                break;
        }
    }

    public void Move(Vector2Int TargetPosition)
    {
        if (currentState == State.Idle || currentState == State.Attack)
        {

            // Current object position must be startPosition to use A* Algorithm.
            // So I take the world position of the grid where our object is located to send it to PathFinder. 
            var startPosition = GridManager.Instance.GetGridWorldPosition(gameObject.transform.position);

            // Get closest path from PathFinder.
            // StartPosition = Object position
            // TargetPosition = The clicked position we get from SoldierInteractManager.
            List<Node> path = PathFinder.Instance.FindPath(startPosition, TargetPosition);

            StartCoroutine(MoveCoroutine(gameObject, path));
        }
    }

    // Provides the movement of the object piece by piece according to Distance.
    private IEnumerator MoveCoroutine(GameObject myObject, List<Node> path)
    {
        if (path != null)
        {
            isWalking = true;

            foreach (Node node in path)
            {
                isWalking = true;
                Vector3 targetPosition = new Vector3(node.Position.x * GridManager.Instance.CellSize, node.Position.y * GridManager.Instance.CellSize, 0);
                while (Vector3.Distance(myObject.transform.position, targetPosition) > 0.01f)
                {
                    myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, targetPosition, MOVE_SPEED * Time.deltaTime);

                    yield return null;
                }
            }
        }

        // Last grid cell must be used and Walk state change to Idle state.
        Node lastCell = path[path.Count - 1];
        lastCell.IsUsed = true;
        isWalking = false;

    }



}
