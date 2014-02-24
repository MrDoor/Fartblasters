using UnityEngine;
using System.Collections;

enum Direction
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN
};

public class Constants 
{
    // These are hard coded to the values of the layers in the editor
    // Have to update by hand if they change
    public const int BLOCKLAYER_DEFAULT     = 1 << 11;
    public const int BLOCKLAYER_SLIPPERY    = 1 << 12;
    public const int BLOCKLAYER_BOUNCY      = 1 << 13;
    public const int BLOCKLAYER_STICKY      = 1 << 14;
    public const int BLOCKLAYER_TELEPORT1   = 1 << 15;
    public const int BLOCKLAYER_TELEPORT2   = 1 << 16;
    public const int BLOCKLAYER_STOP        = 1 << 17;
    public const int BLOCKLAYER_MAGNET      = 1 << 18;
    public const int BLOCKLAYER_VORTEX      = 1 << 19;
    public const int BLOCKLAYER_DISAPPEAR   = 1 << 20;  
    public const int BLOCKLAYER_MOVING      = 1 << 21;
    public const int BLOCKLAYER_FALLING     = 1 << 22;
    public const int BLOCKLAYER_BOOST       = 1 << 23;
    
    
	public const int BLOCKLAYER_GROUND      = 1 << 26;

    // Bit flag mask for blocks that you can land on
    public const int LayerMask_Ground = BLOCKLAYER_DEFAULT      | 
                                        BLOCKLAYER_SLIPPERY     | 
                                        BLOCKLAYER_STICKY       | 
                                        BLOCKLAYER_STOP         |
                                        BLOCKLAYER_DISAPPEAR    | 
                                        BLOCKLAYER_MOVING       | 
                                        BLOCKLAYER_FALLING		|
                                        BLOCKLAYER_GROUND;
}