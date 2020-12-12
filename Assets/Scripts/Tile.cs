using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TaquinController GlobTaquinController;
    private Rigidbody2D SelfRigidbody;
    private Transform SelfTransform;

    public float SnapSpeed;
    public float MoveSpeed;

    private Vector3 SnapPos = new Vector3();
    private Vector2 TouchStartPosition = new Vector2();

    private void Awake()
    {
        GlobTaquinController = FindObjectOfType<TaquinController>();
        SelfRigidbody = GetComponent<Rigidbody2D>();
        SelfTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            Snap();
        }
    }

    private void Snap()
    {
        SnapPos.x = (int)(SelfTransform.position.x + 0.5);
        SnapPos.y = (int)(SelfTransform.position.y + 0.5);

        Vector3 direction = SnapPos - SelfTransform.position;

        SelfTransform.Translate(direction * SnapSpeed * Time.deltaTime);
    }

    public void Move()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                TouchStartPosition = touch.position;
                break;

            case TouchPhase.Stationary:
                TouchStartPosition = touch.position;
                break;

            case TouchPhase.Moved:
                Vector2 direction = touch.position - TouchStartPosition;
                SelfRigidbody.velocity = direction * MoveSpeed * Time.fixedDeltaTime;
                break;

            case TouchPhase.Ended:
                GlobTaquinController.NewTilesPosition();
                break;
        }
    }

    public Vector2Int GetPosition()
    {
        Snap();

        Vector2Int result = new Vector2Int();
        result.x = (int)SnapPos.x;
        result.y = 2 - (int)SnapPos.y;

        return result;
    }
}