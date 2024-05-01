public class Stone : Item {
    private int maxStackSize = 5;
    private double weight = 5;
    public int amount;

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }
    public override int Amount { get => amount; set => amount = value; }
}
