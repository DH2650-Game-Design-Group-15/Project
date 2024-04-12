using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Wood : Item {
    private int maxStackSize = 10;
    private double weight = 2;

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }

    void Start(){
        Amount = 1;
    }
}
