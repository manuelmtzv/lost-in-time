

public class Damage
{
    public int DamageValue { get; set; }
    public bool IsCritical { get; set; }

    public Damage(int damageValue, bool isCritical)
    {
        DamageValue = damageValue;
        IsCritical = isCritical;
    }
}