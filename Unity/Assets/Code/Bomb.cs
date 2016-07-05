using UnityEngine;
using System.Collections;
using System;

public class Bomb : MonoBehaviour
{
    #region Fields

    public Explosion ExplosionPrefab;
    public Collider2D collider;

    public int BaseRange = 3;
    public int BaseDamage = 1;
    public float DetonateTime = 3.0f;

    private Animator anim;
    private int animExplode = Animator.StringToHash("Explode");
    private int animRefresh = Animator.StringToHash("Refresh");
    private int animExplodedShortState = Animator.StringToHash("Exploded");

    private int range = 0;
    private int damage = 0;
    private float detonateTime = 0;
    private Grid grid = null;
    private bool exploded = false;

    private Bomber bomber = null;

    private Vector2 bombGridCoord = Vector2.zero;

    #endregion

    #region Awake

    public void Awake()
    {
        anim = GetComponent<Animator>();
        collider.enabled = true;
    }

    #endregion

    #region Detonate

    public void Detonate(Grid grid, Bomber bomber, Vector2 gridCoord, int extraRange = 0, int extraDamage = 0, float DetonateOverride = -1)
    {
        // Set variables
        bombGridCoord = gridCoord;
        range = BaseRange + extraRange;
        damage = BaseDamage + extraDamage;
        detonateTime = DetonateOverride != -1 ? DetonateOverride : DetonateTime;
        this.grid = grid;
        this.bomber = bomber;

        // refresh some values
        collider.enabled = false;
        exploded = false;
        anim.SetTrigger(animRefresh);

        // Start Bomb Coroutine
        StartCoroutine(DetonateCR());
    }

    private IEnumerator DetonateCR()
    {
        // Wait for detonation
        yield return new WaitForSeconds(detonateTime);

        if (exploded)
            yield break;

        // Trigger spawning of explosion objects & cleanup
        DetonateNow();
    }

    public void DetonateNow()
    {
        // Set anim
        anim.SetTrigger(animExplode);
        exploded = true;

        Camera.main.GetComponent<Shake>().StartShake();

        // Spawn explosions
        // Center pos
        Vector2 gridCoord = grid.GetGridCoordinates(transform.position, Grid.SnapSpotVer.Mid,Grid.SnapSpotHor.Left); //bombGridCoord; grid.GetGridCoordinates(transform.position);
        if (Spawn(ExplosionPrefab, grid, gridCoord, Explosion.ExplosionRotation.Center, Explosion.ExplosionType.Center))
        {
            // Spawn in four directions if initial blas is succesfull
            bool left = true, right = true, top = true, bottom = true;
            for (int i = 1; i <= range; i++) // If one cannot spawn - stop it from happening
            {
                Explosion.ExplosionType type = i == range ? Explosion.ExplosionType.End : Explosion.ExplosionType.Mid;
                if (top) top = Spawn(ExplosionPrefab, grid, gridCoord + new Vector2(0, i), Explosion.ExplosionRotation.Top, type);
                if (bottom) bottom = Spawn(ExplosionPrefab, grid, gridCoord + new Vector2(0, -i), Explosion.ExplosionRotation.Bottom, type);
                if (right) right = Spawn(ExplosionPrefab, grid, gridCoord + new Vector2(i, 0), Explosion.ExplosionRotation.Right, type);
                if (left) left = Spawn(ExplosionPrefab, grid, gridCoord + new Vector2(-i, 0), Explosion.ExplosionRotation.Left, type);
            }
        }

        //StartCoroutine(CleanUpCR());
        CleanUp();
    }

    //private IEnumerator CleanUpCR()
    //{
    //    // Wait to destroy Self
    //    AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
    //    while (info.shortNameHash != animExplodedShortState)
    //    {
    //        info = anim.GetCurrentAnimatorStateInfo(0);
    //        yield return null;
    //    }

    //    CleanUp();
    //}

    private void CleanUp()
    {
        //// Destroy Self & make another bomb available
        StopAllCoroutines();
        bomber.AvailableBombs++;
        GameObject.Destroy(this.gameObject);
    }

    #endregion

    #region Spawn

    private bool Spawn(Explosion explosion, Grid grid, Vector2 gridPos, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        //Debug.Log(gridPos);

        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= grid.GridWidth || gridPos.y >= grid.GridHeight)
        {
            //Debug.Log("Explosion grid location not legal - out of range " + gridPos.x + " - " + gridPos.y);
            return false;
        }

        // Check if Legal - Take into account grid types
        // Not a floor
        GridElement element = grid.GetGridElement((int)gridPos.x, (int)gridPos.y);
        if (element.Type != GridElement.GridType.Floor)
        {
            // Not legal
            //Debug.Log("Explosion grid location not legal " + element.Type + " pos: " + gridPos.x + " - " + gridPos.y);
            return false;
        }

        // Transform to worldpos
        Vector3 worldPos = grid.GetGridWorldPos((int)gridPos.x, (int)gridPos.y) + new Vector3(Grid.TileWidth / 2, 0);

        // A block is a special case where it cannot continue afterwards, but still spawn an explosion
        GridElement block = grid.GetBlockElement((int)gridPos.x, (int)gridPos.y);
        if (block != null && block.Type == GridElement.GridType.Block)
        {
            // Trigger block destroy
            Debug.Log("Destroy Block plz");
            block.GetComponent<Destructable>().StartDestruction();
            // Trigger explosion
            Spawn(explosion, worldPos, rotation, type);
            return false;
        }

        // Spawn object
        Spawn(explosion, worldPos, rotation, type);
        return true;
    }


    private void Spawn(Explosion explosion, Vector3 position, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        // Spawn Object
        GameObject go = (GameObject.Instantiate(explosion.gameObject, position, Quaternion.identity) as GameObject);
        Explosion newExplode = go.GetComponent<Explosion>();
        
        newExplode.Initiate(type, rotation, damage);
    }

    #endregion

    void OnTriggerEnter2D(Collider2D other)
    {
        Bomber colBomber = other.GetComponent<Bomber>();
        if(colBomber != null)
        {
            colBomber.OnBomb = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetInstanceID() == bomber.gameObject.GetInstanceID())
        {
            collider.enabled = true;
        }

        Bomber colBomber = other.GetComponent<Bomber>();
        if (colBomber != null)
        {
            colBomber.OnBomb = false;
        }
    }
}
