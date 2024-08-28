using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Soldier : MonoBehaviour, IDamagable
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

    public int Health;
    public int SoldierID;
    public int Damage;
    public float AttackDelay;
    public TextMeshProUGUI healthText;


    #endregion

    private void Start()
    {
        currentState = State.Idle;
        healthText.text = Health.ToString();
    }

    public abstract void Initialize(int health, int id, int damage, float attackRate);
    public  virtual IEnumerator Attack(GameObject targetObject)
    {
        isAttacking = true;
        while (isAttacking)
        {
            if (targetObject != null)
            {
                targetObject.GetComponent<IDamagable>().TakeDamage(Damage);
                yield return new WaitForSeconds(2f);
            }
            else
            {
                isAttacking = false;
                yield break;
            }
        }
        
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

    public void TakeDamage(int damage)
    {
        Health -= damage;
        healthText.text = Health.ToString();
        if (Health <= 0) 
        {
            Vector2Int position = GridManager.Instance.GetGridWorldPosition(gameObject.transform.position);
            var cell = GridManager.Instance.GetGridCellByPosition(position);
            cell.IsUsed = false;
            Destroy(this.gameObject);
        }
    }

    public void Move(Vector2Int TargetPosition,GameObject interactObject)
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

            StartCoroutine(MoveCoroutine(gameObject, path , interactObject));
        }
    }

    // Provides the movement of the object piece by piece according to Distance.
    private IEnumerator MoveCoroutine(GameObject myObject, List<Node> path, GameObject interactObject)
    {
        if (path != null)
        {
            isAttacking = false;
            isWalking = true;
            foreach (Node node in path)
            {
                isWalking = true;
                Vector3 targetPosition = new Vector3(node.Position.x * GridManager.Instance.CellSize, node.Position.y * GridManager.Instance.CellSize, 0);
                while (Vector3.Distance(myObject.transform.position, targetPosition) > 0.001f)
                {
                    myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, targetPosition, MOVE_SPEED * Time.deltaTime);

                    yield return null;
                }
            }
            isWalking = false;

            if (interactObject.GetComponent<IDamagable>() != null)
                StartCoroutine(Attack(interactObject));
        }


        // Last grid cell must be used and Walk state change to Idle state.


    }
}
