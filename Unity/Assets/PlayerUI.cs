using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public Player Player;
    public Text Lifes;
    public GameObject BombContainer;
    public Sprite FullBomb, EmptyBomb;
    public Text BombRange, Speed;

    private Bomber bomber;
    private int lastBombCount;
    private int lastAvailableCount;

    public void Awake()
    {
        if (Player == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        bomber = Player.GetComponent<Bomber>();
    }

    public void Update()
    {
        Lifes.text = Player.Lives.ToString() + "X";
        BombRange.text = bomber.BonusRange.ToString();
        Speed.text = "0";

        // Check if bombsgrid need to be updated
        if(bomber.AvailableBombs != lastAvailableCount || bomber.MaxBombs != lastBombCount)
        {

            Image BombRenderer;// = BombContainer.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < BombContainer.transform.childCount; i++)
            {
                BombRenderer = BombContainer.transform.GetChild(i).GetComponent<Image>();
                // swap sprites
                if (i < bomber.AvailableBombs)
                    BombRenderer.sprite = FullBomb;
                else
                    BombRenderer.sprite = EmptyBomb;

                // Hide all the bombs above maxbombs
                if (i < bomber.MaxBombs)
                    BombRenderer.gameObject.SetActive(true);
                else
                    BombRenderer.gameObject.SetActive(false);
            }
            lastAvailableCount = bomber.AvailableBombs;
            lastBombCount = bomber.MaxBombs;
        }
    }
}
