using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Item {
    private int maxStackSize = 10;
    private double weight = 2;
    public int getMaxStackSize(){
        return maxStackSize;
    }
    public double getWeight(){
        return weight;
    }
}
