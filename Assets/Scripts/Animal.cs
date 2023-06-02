using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class Animal : ScriptableObject
{
    /*Attention
    Hunger
        Energy
    Cleanliness
        Health (HP)*/
    [SerializeField] private float maxAttention = 100;
    [SerializeField] private float maxHunger = 100;
    [SerializeField] private float maxEnergy = 100;
    [SerializeField] private float maxCleanliness = 100;
    [SerializeField] private float maxHealth = 100;
    
    [Header("Rates Of Change")]
    //This is how many times per second you want it to reduce by 1
    [SerializeField] private float attRate;
    [SerializeField] private float hunRate;
    [SerializeField] private float energyRate;
    [SerializeField] private float cleanRate;
    //[SerializeField] private float healthRate;

    public float MaxAttention
    {
        get => maxAttention;
        set => maxAttention = value;
    }

    public float MaxHunger
    {
        get => maxHunger;
        set => maxHunger = value;
    }

    public float MaxEnergy
    {
        get => maxEnergy;
        set => maxEnergy = value;
    }

    public float MaxCleanliness
    {
        get => maxCleanliness;
        set => maxCleanliness = value;
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float AttRate
    {
        get => attRate/5f;
        set => attRate = value;
    }

    public float HunRate
    {
        get => hunRate/5f;
        set => hunRate = value;
    }

    public float EnergyRate
    {
        get => energyRate/5f;
        set => energyRate = value;
    }

    public float CleanRate
    {
        get => cleanRate/5f;
        set => cleanRate = value;
    }

    /*public float HealthRate
    {
        get => healthRate/5f;
        set => healthRate = value;
    }*/
}
