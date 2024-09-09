using Godot;
using System;

public interface IShieldCollectible
{
    public int GetPenetrationLevel();
    public void OnCatch();
    public void ShootBack(float boostLevel = 1.5f);
    public bool IsFiredFromShield();
    //public void SetIsFiredFromShield(bool value);
}
