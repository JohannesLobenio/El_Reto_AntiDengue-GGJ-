using UnityEngine;

public class Raid : Item
{
    float currentTime;
    public override void Equip()
    {
      
    }
    public override void OnUse()
    {
        currentTime += Time.deltaTime;
    }
    public override void Use()
    {

    }
}

public abstract class Item
{
    public abstract void Equip();
    public abstract void OnUse();
    public abstract void Use();
}
