using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{

    // Tilemaps needed to draw/erase tiles from them map
    [SerializeField] Tilemap terrain;
    [SerializeField] Tilemap backgroundTerrain;
    [SerializeField] float tileScale = 0.125f;
    [SerializeField] GameObject explosionFx;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionDuration;

    [Space]
    [Header ("Debug Stuff (let's keep it in for now")]

    [SerializeField] Tilemap debugMap;
    [SerializeField] TileBase sourceTile;
    [SerializeField] TileBase targetTile;
    [SerializeField] TileBase unblockedTile;
    [SerializeField] TileBase blockerTile;



    // Singleton instance
    public static TerrainDestroyer instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

    }

    // Let's keep this in for now. It was really useful
    // DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        for (int x = debugMap.cellBounds.min.x; x < debugMap.cellBounds.max.x; x++)
    //        {
    //            for (int y = debugMap.cellBounds.min.y; y < debugMap.cellBounds.max.y; y++)
    //            {
    //                Vector3Int tilePosition = new Vector3Int(x, y, 0);
    //                TileBase tile = debugMap.GetTile(tilePosition);
    //                if (tile != null)
    //                {
    //                    debugMap.SetTile(tilePosition, null);
    //                }
    //            }

    //        }
    //    }
    //}
    // DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG

    // Destroys terrain, called from the Bomb with specific position
    public void DestroyTerrain_Bomb(Vector3 translatedPosition) {
        Vector3Int tilePos = terrain.WorldToCell(translatedPosition);
        if (terrain.GetTile(tilePos) != null) {
            DestroyTile(tilePos);
        }
    }

    public void DestoryTerrain_Positions(List<Vector3> translatedPositions, Vector3 explosionSource)
    {
        var uniqueTiles = new HashSet<Vector3Int>();
        var durableTiles = new HashSet<Vector3Int>();

        foreach (Vector3 position in translatedPositions)
        {
            // grab the tile and turn it into a cell position 
            Vector3Int tilePos = terrain.WorldToCell(position);

            // check for durable tiles
            GameObject gameObject = terrain.GetInstantiatedObject(tilePos);
            if (gameObject != null)
            {
                DurableEarthScript durability;
                gameObject.TryGetComponent<DurableEarthScript>(out durability);
                if (durability != null)
                {
                    durableTiles.Add(tilePos);
                }
            }
            else
            {
                // add the tile to the unique tile set
                uniqueTiles.Add(tilePos);
            }

        }

        // hold on to a set of tiles that are not protected from the blast
        var uniqueUnprotectedTiles = new HashSet<Vector3Int>();

        // calculate for each tile
        // determine if a line drawn from center of the explosion to the center of the target tile passes throuhg a normal tile
        foreach (Vector3Int position in uniqueTiles)
        {

            // translate the tile back into the world
            Vector3 worldLocationNormalTile = terrain.CellToWorld(position);

            bool isProtected = true;

            // for each durable tile check to see if it "blocks" the explosion
            foreach (Vector3Int durableTilePosition in durableTiles)
            {
                // translate the tile back into the world
                Vector3 worldLocationDurableTile = terrain.CellToWorld(durableTilePosition);

                // create a square of that durable tile
                Square durableTileSquare = new Square(worldLocationDurableTile, terrain.cellSize.x * tileScale);

                bool explosionCenterToNormalTile = Line.SegmentIntersectRectangle(durableTileSquare.Left, durableTileSquare.Bottom, durableTileSquare.Right, durableTileSquare.Top, explosionSource.x + (terrain.cellSize.x * tileScale/2), explosionSource.y + (terrain.cellSize.y * tileScale/2), worldLocationNormalTile.x, worldLocationNormalTile.y);

                if (explosionCenterToNormalTile)
                {
                    // grab the tile and turn it into a cell position 
                    Vector3Int protectedTilePos = terrain.WorldToCell(position);

                    //isProtected = false;
                    isProtected = false;

                }
                // Let's leave this in for DEBUG purposes
                else
                {
                    //Vector3Int durPos = debugMap.WorldToCell(worldLocationDurableTile);
                    //debugMap.SetTile(durPos, blockerTile);
                    //isProtected = true;
                }
            }

            if (isProtected) {
                uniqueUnprotectedTiles.Add(position);
            }
        }

        // Let's leave this in for DEBUG purposes
        // DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG
        //foreach (Vector3Int position in durableTiles)
        //{
        //    debugMap.SetTile(position, blockerTile);
        //}
        //foreach (Vector3Int position in uniqueUnprotectedTiles)
        //{
        //    debugMap.SetTile(position, targetTile);
        //}
        //// remove stuff
        //uniqueTiles.ExceptWith(uniqueUnprotectedTiles);
        //foreach (Vector3Int position in uniqueTiles)
        //{
        //    debugMap.SetTile(position, unblockedTile);
        //}

        //Vector3Int explodeSourceTile = debugMap.WorldToCell(explosionSource);
        //debugMap.SetTile(explodeSourceTile, null);
        //debugMap.SetTile(explodeSourceTile, sourceTile);
        //// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG// DEBUG


        // now for each cell call this once.
        // combine the unprotected cells and the durable tiles and explode them each individually
        uniqueUnprotectedTiles.UnionWith(durableTiles);
        foreach (Vector3Int cellPosition in uniqueUnprotectedTiles)
        {
            DestroyTile(cellPosition);
        }

    }

    // Sets the tile in the foreground to nil.
    // also reduces the durability of Durable tiles by 1
    void DestroyTile(Vector3Int tilePosition)
    {

        // spwan an explosion
        Vector3 explosionWorldPosition = terrain.CellToWorld(tilePosition);
        GameObject explosion = Instantiate(explosionFx, explosionWorldPosition, Quaternion.identity);
        explosion.transform.localScale *= (transform.localScale.x + (explosionRadius + 1));
        Destroy(explosion, explosionDuration);

        // destroy the tile
        GameObject gameObject = terrain.GetInstantiatedObject(tilePosition);
        if (gameObject != null)
        { 
            DurableEarthScript durability;
            gameObject.TryGetComponent<DurableEarthScript>(out durability);
            if (durability != null) {
                durability.durability--;

                // early return because the Durable tile still has "life" yet
                if (durability.durability > 0)
                {
                    return;
                }

            }
        }
        terrain.SetTile(tilePosition, null);
    }
}


// Yay, Geometry!!!
public struct Square
{
    public float Left   { get { return _center.x - _sideLength / 2; } }
    public float Right  { get { return _center.x + _sideLength / 2; } }
    public float Top    { get { return _center.y + _sideLength / 2; } }
    public float Bottom { get { return _center.y - _sideLength / 2; } }

    private Vector3 _center;
    public float _sideLength;

    public Square(Vector3 center, float sideLength)
    {
        _center = center;
        _sideLength = sideLength;
    }
}

public struct Line
{

    public static bool SegmentIntersectRectangle(
        double rectangleMinX,
        double rectangleMinY,
        double rectangleMaxX,
        double rectangleMaxY,
        double p1X,
        double p1Y,
        double p2X,
        double p2Y)
    {
        // Find min and max X for the segment
        double minX = p1X;
        double maxX = p2X;

        if (p1X > p2X)
        {
            minX = p2X;
            maxX = p1X;
        }

        // Find the intersection of the segment's and rectangle's x-projections
        if (maxX > rectangleMaxX)
        {
            maxX = rectangleMaxX;
        }

        if (minX < rectangleMinX)
        {
            minX = rectangleMinX;
        }

        if (minX > maxX) // If their projections do not intersect return false
        {
            return false;
        }

        // Find corresponding min and max Y for min and max X we found before
        double minY = p1Y;
        double maxY = p2Y;

        double dx = p2X - p1X;

        if (Mathf.Abs((float)dx) > 0.0000001)
        {
            double a = (p2Y - p1Y) / dx;
            double b = p1Y - a * p1X;
            minY = a * minX + b;
            maxY = a * maxX + b;
        }

        if (minY > maxY)
        {
            double tmp = maxY;
            maxY = minY;
            minY = tmp;
        }

        // Find the intersection of the segment's and rectangle's y-projections
        if (maxY > rectangleMaxY)
        {
            maxY = rectangleMaxY;
        }

        if (minY < rectangleMinY)
        {
            minY = rectangleMinY;
        }

        if (minY > maxY) // If Y-projections do not intersect return false
        {
            return false;
        }

        return true;
    }

}


