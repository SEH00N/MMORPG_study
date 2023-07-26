using UnityEngine;

public class CursorController : MonoBehaviour
{
    private CursorType cursorType = CursorType.None;

    private Texture2D attackIcon;
    private Texture2D handIcon;

    private int mask = (int)DEFINE.Layer.Ground | (int)DEFINE.Layer.Monster;

    private void Start()
    {
        attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    private void Update()
    {
        UpdateMouseCursor();
    }

    private void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            if (1 << hit.collider.gameObject.layer == (int)DEFINE.Layer.Monster)
            {
                if (cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width / 5, 0), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(handIcon, new Vector2(handIcon.width / 3, 0), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }
            }
        }
    }

    public enum CursorType
    {
        None,
        Attack,
        Hand
    }
}
