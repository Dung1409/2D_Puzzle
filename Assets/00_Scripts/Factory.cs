using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Factory
{

    public static Block createBlock(string type)
    {
        switch (type)
        {
            case "Block":
                return new Block();
            // Add more cases for different block types if needed
            default:
                Debug.LogError("Unknown block type: " + type);
                return null;
        }
    }
}
