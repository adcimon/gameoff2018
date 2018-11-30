using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class BombController : MonoBehaviour
{
    public KeyCode bombKey = KeyCode.Q;
    public GameObject bomb;
    public float offset = 0;

    private MovementController movementController;
    private GameObject activeBomb;

    private void Awake()
    {
        movementController = gameObject.GetComponent<MovementController>();
    }

    private void Update()
    {
        // Check if the bomb key is pressed.
        if( !Input.GetKeyDown(bombKey) )
        {
            return;
        }

        // Check if there is already a bomb.
        if( activeBomb )
        {
            return;
        }

        PlaceBomb();
    }

    private void PlaceBomb()
    {
        if( !bomb )
        {
            return;
        }

        activeBomb = Instantiate(bomb);
        activeBomb.transform.position = gameObject.transform.position + movementController.faceDirection * offset;
    }
}