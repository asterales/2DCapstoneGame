using UnityEngine;
using System.Collections;

// This class stores the dimensions of a hex -> used for position calculations
// Only need one reference to this because all hexes will be identical

public class HexDimension : MonoBehaviour {
    //     a
    //    / \
    //   /   \
    //  /     \
    // v   b   d
    // |   c   |
    // v       v
    //  \     /
    //   \   /
    //    \ /
    //     v

    // terrible depiction but v's are verts of hex
    // 'c' is center
    // 'b' is aligned with non apex verts
    // 'a' and 'd' are vert
    // when a hex is regular, the apex can be any vert

    public float apex;       // dist from c to a
    public float minorApex;  // dist from c to b
    public float width;      // dist from b to d

    public float globalTopLeftX = -5.0f;
    public float globalTopLeftY = 5.0f;

    // all other lengths or angles can be calculated with the above values
}
