using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditMode
{
    [Test]
    public void Shoot_ReduceCurrentAmmoByPellets()
    {
        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
        weapon.currAmmo = 0;

        weapon.AddAmmo();

        Assert.AreEqual(Mathf.Infinity, weapon.currAmmo);
    }

    [Test]
    public void Shoot_FastFireActivation()
    {
        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
        weapon.fastFire = false;

        weapon.FastFireOn();

        Assert.IsTrue(weapon.fastFire);
    }

    [Test]
    public void TurnOffActivatedSpikes()
    {
        var gameObject = new GameObject();
        var spikes = gameObject.AddComponent<Spikes>();
        var animator = gameObject.AddComponent<Animator>();
        spikes.animator = animator;
        spikes.activated = true;

        spikes.TurnOff();
        
        Assert.IsFalse(spikes.activated);
        Assert.IsFalse(spikes.animator.GetBool("Spikes"));
    }

    [Test]
    public void SwitchOnPropsAltar()
    {
        var gameObject = new GameObject();
        var altar = gameObject.AddComponent<PropsAltar>();
        altar.start = true;

        altar.Switch();

        Assert.IsFalse(altar.start);
    }
}
