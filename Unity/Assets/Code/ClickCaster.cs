using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Bomber))]
public class ClickCaster : MonoBehaviour
{
    public Text DebugText;

    private Bomber bomber;
    private Grid grid;
    private Player[] players;

    public void Start()
    {
        bomber = GetComponent<Bomber>();
        grid = FindObjectOfType<Grid>();
        players = FindObjectsOfType<Player>();
    }

    public void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        //mousePos.z = 0;
        
        if(DebugText != null)
        {
            DebugText.text = Input.mousePosition.ToString() + " --- " + mousePos.ToString();
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Create bomb
            Debug.Log("Create bomb by click " + mousePos);
            bomber.DropBomb(grid, mousePos);
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Create wall

        }
    }
}
