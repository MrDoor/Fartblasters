using UnityEngine;
using System.Collections;

public enum Direction
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

    public const int LAYER_INDEX_ENEMIES = 9;
    public const int LAYER_INDEX_DEFAULT_BLOCK = 11;
    public const int LAYER_INDEX_SLIPPERY_BLOCK = 12;
    public const int LAYER_INDEX_BOUNCY_BLOCK = 13;
    public const int LAYER_INDEX_STICKY_BLOCK = 14;
    public const int LAYER_INDEX_TELEPORT1_BLOCK = 15;
    public const int LAYER_INDEX_TELEPORT2_BLOCK = 16;
    public const int LAYER_INDEX_STOP_BLOCK = 17;
    public const int LAYER_INDEX_MAGNET_BLOCK = 18;
    public const int LAYER_INDEX_VORTEX_BLOCK = 19;
    public const int LAYER_INDEX_DISAPPEAR_BLOCK = 20;
    public const int LAYER_INDEX_MOVING_BLOCK = 21;
    public const int LAYER_INDEX_FALLING_BLOCK = 22;
    public const int LAYER_INDEX_BOOST_BLOCK = 23;

    public const int LAYER_INDEX_WALL = 25;
    public const int LAYER_INDEX_GROUND_BLOCK = 26;

    public const int LAYER_INDEX_TRAJECTORY_DOT = 31;

    //------------------------------------------------------    

    public const int LAYER_MASK_ENEMIES = 1 << LAYER_INDEX_ENEMIES;
    public const int LAYER_MASK_DEFAULT_BLOCK = 1 << LAYER_INDEX_DEFAULT_BLOCK;
    public const int LAYER_MASK_SLIPPERY_BLOCK = 1 << LAYER_INDEX_SLIPPERY_BLOCK;
    public const int LAYER_MASK_BOUNCY_BLOCK = 1 << LAYER_INDEX_BOUNCY_BLOCK;
    public const int LAYER_MASK_STICKY_BLOCK = 1 << LAYER_INDEX_STICKY_BLOCK;
    public const int LAYER_MASK_TELEPORT1_BLOCK = 1 << LAYER_INDEX_TELEPORT1_BLOCK;
    public const int LAYER_MASK_TELEPORT2_BLOCK = 1 << LAYER_INDEX_TELEPORT2_BLOCK;
    public const int LAYER_MASK_STOP_BLOCK = 1 << LAYER_INDEX_STOP_BLOCK;
    public const int LAYER_MASK_MAGNET_BLOCK = 1 << LAYER_INDEX_MAGNET_BLOCK;
    public const int LAYER_MASK_VORTEX_BLOCK = 1 << LAYER_INDEX_VORTEX_BLOCK;
    public const int LAYER_MASK_DISAPPEAR_BLOCK = 1 << LAYER_INDEX_DISAPPEAR_BLOCK;
    public const int LAYER_MASK_MOVING_BLOCK = 1 << LAYER_INDEX_MOVING_BLOCK;
    public const int LAYER_MASK_FALLING_BLOCK = 1 << LAYER_INDEX_FALLING_BLOCK;
    public const int LAYER_MASK_BOOST_BLOCK = 1 << LAYER_INDEX_BOOST_BLOCK;

    public const int LAYER_MASK_WALL = 1 << LAYER_INDEX_WALL;
    public const int LAYER_MASK_GROUND_BLOCK = 1 << LAYER_INDEX_GROUND_BLOCK;

    public const int LAYER_MASK_TRAJECTORY_DOT = 1 << LAYER_INDEX_TRAJECTORY_DOT;

    // Bit flag mask for blocks that you can land on
    public const int LayerMask_Ground = LAYER_MASK_ENEMIES |
                                        LAYER_MASK_DEFAULT_BLOCK |
                                        LAYER_MASK_SLIPPERY_BLOCK |
                                        LAYER_MASK_STICKY_BLOCK |
                                        LAYER_MASK_STOP_BLOCK |
                                        LAYER_MASK_DISAPPEAR_BLOCK |
                                        LAYER_MASK_MOVING_BLOCK |
                                        LAYER_MASK_FALLING_BLOCK |
                                        LAYER_MASK_WALL |
                                        LAYER_MASK_GROUND_BLOCK;

    public const string MAIN_MENU = "StartScreen";
}