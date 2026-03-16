using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

//[CreateAssetMenu]
[CreateAssetMenu (menuName = "Custom Tiles/Banner Rule Tile")]
public class BannerRuleTile : RuleTile<BannerRuleTile.Neighbor> {
    public bool alwaysConnect;
    public TileBase[] tilesToConnect;

    public bool checkSelf;

    public class Neighbor : RuleTile.TilingRule.Neighbor {

        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
        
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return checkThis(tile);
            case Neighbor.NotThis: return checkNotThis(tile);
            case Neighbor.Any: return checkAny(tile);
            case Neighbor.Specified: return checkSpecified(tile);
            case Neighbor.Nothing: return checkNothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool checkNothing(TileBase tile)
    {
        return tile == null;
    }

    private bool checkSpecified(TileBase tile)
    {
        return tilesToConnect.Contains(tile);
    }

    private bool checkAny(TileBase tile)
    {
        if(checkSelf == true)
        {
            return tile != null;
        }
        else
        {
            return tile != null && tile != this;
        }
    }

    private bool checkNotThis(TileBase tile)
    {
        return tile != this;
    }

    private bool checkThis(TileBase tile)
    {
        if(alwaysConnect == false)
        {
            return tile == this;
        }
        else
        {
            return tilesToConnect.Contains(tile) || tile == this;
        }
    }
}