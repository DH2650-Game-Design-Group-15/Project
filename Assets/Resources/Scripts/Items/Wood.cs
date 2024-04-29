public class Wood : Item {
    private int maxStackSize = 10;
    private double weight = 2;
    public int amount;

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }
    public override int Amount { get => amount; set => amount = value; }
}
