using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D[] cursorImg;

    void Start()
    {
        Cursor.SetCursor(cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
    }
}
