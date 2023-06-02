using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHandler : MonoBehaviour
{
    /*Attention
    Hunger
        Energy
    Cleanliness
        Health (HP)*/
    [Header("Sliders")]
    [SerializeField] private Slider SLAttention;
    [SerializeField] private Slider SLHunger;
    [SerializeField] private Slider SLEnergy;
    [SerializeField] private Slider SLCleanliness;
    [SerializeField] private Slider SLHealth;
    
    [Header("Stats")]
    [SerializeField] private Animal animalStats;

    public Animal AnimalStats
    {
        get => animalStats;
        set => animalStats = value;
    }
    
    [SerializeField] private float currentAttention, currentHunger, currentEnergy, currentCleanliness, currentHealth;
    [SerializeField] private float healthRate;

    [Header("Game elements")]
    [SerializeField] private bool occupied;

    [SerializeField] private bool dead;

    public bool Dead
    {
        get => dead;
        set => dead = value;
    }

    public bool Occupied
    {
        get => occupied;
        set => occupied = value;
    }

    public float CurrentAttention
    {
        get => currentAttention;
        set => currentAttention = value;
    }

    public float CurrentHunger
    {
        get => currentHunger;
        set => currentHunger = value;
    }

    public float CurrentEnergy
    {
        get => currentEnergy;
        set => currentEnergy = value;
    }

    public float CurrentCleanliness
    {
        get => currentCleanliness;
        set => currentCleanliness = value;
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public StatBlock getStats()
    {
        StatBlock temp = new StatBlock(currentAttention, currentHunger, currentEnergy, currentCleanliness,
            currentHealth);
        return temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        occupied = false;
        dead = false;
        SetMaxSliders(animalStats.MaxAttention, animalStats.MaxHunger, animalStats.MaxEnergy, animalStats.MaxCleanliness, animalStats.MaxHealth);
        SetStats(animalStats.MaxAttention, animalStats.MaxHunger, animalStats.MaxEnergy, animalStats.MaxCleanliness, animalStats.MaxHealth);
        StartCoroutine(LowerStats());
    }

    private void SetMaxSliders(float attention, float hunger, float energy, float cleanliness, float health)
    {
        SLAttention.maxValue = attention;
        SLHunger.maxValue = hunger;
        SLEnergy.maxValue = energy;
        SLCleanliness.maxValue = cleanliness;
        SLHealth.maxValue = health;
    }
    
    private void SetSliders(float attention, float hunger, float energy, float cleanliness, float health)
    {
        SLAttention.value = attention;
        SLHunger.value = hunger;
        SLEnergy.value = energy;
        SLCleanliness.value = cleanliness;
        SLHealth.value = health;
    }

    private IEnumerator LowerStats()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(0.2f);
            if (!occupied)
            {
                float newAttention = (float)(Math.Round((currentAttention - animalStats.AttRate) * 100f) / 100f);
                float newHunger = (float)(Math.Round((currentHunger - animalStats.HunRate) * 100f) / 100f);
                float newCleanliness = (float)(Math.Round((currentCleanliness - animalStats.CleanRate) * 100f) / 100f);
                float newEnergy = (float)(Math.Round((currentEnergy - animalStats.EnergyRate) * 100f) / 100f);

                newAttention = Clamp(newAttention);
                newHunger = Clamp(newHunger);
                newCleanliness = Clamp(newCleanliness);
                newEnergy = Clamp(newEnergy);

                healthRate = 0;
                var statVals = new[] { newAttention, newHunger, newEnergy, newCleanliness };
                bool allHealthy = true;
                for (int i = 0; i < statVals.Length; i++)
                {
                    if (!Healthy(statVals[i], i))
                    {
                        healthRate += 0.5f;
                        allHealthy = false;
                    }
                }

                if (allHealthy)
                {
                    healthRate = -1f;
                }

                float newHealth = (float)(Math.Round((currentHealth - healthRate) * 100f) / 100f);
                SetStats(newAttention, newHunger, newEnergy, newCleanliness, newHealth);

                if (newHealth <= 0)
                {
                    dead = true;
                    SetStats(0,0,0,0,0);
                    GetComponentInChildren<CircleCollider2D>().enabled = false;
                    GetComponent<Animator>().Play("Dead");
                }
            }
        }
    }

    public void SetStats(float attention, float hunger, float energy, float cleanliness, float health)
    {
        currentAttention = attention;
        currentHunger = hunger;
        currentCleanliness = cleanliness;
        currentHealth = Clamp(health);
        currentEnergy = energy;
        SetSliders(attention, hunger, energy, cleanliness, health);
    }

    public void SetStats(StatBlock sb)
    {
        currentAttention = sb.Attention;
        currentHunger = sb.Hunger;
        currentCleanliness = sb.Cleanliness;
        currentHealth = Clamp(sb.Health);
        currentEnergy = sb.Energy;
        SetSliders(sb.Attention, sb.Hunger, sb.Energy, sb.Cleanliness, sb.Health);
    }

    private bool Healthy(float stat, int statIndex)
    {
        //newAttention, newHunger, newEnergy, newCleanliness
        float maxCheck = 0;
        switch (statIndex)
        {
            case 0 :
                maxCheck = animalStats.MaxAttention;
                break;
            case 1 :
                maxCheck = animalStats.MaxHunger;
                break;
            case 2:
                maxCheck = animalStats.MaxEnergy;
                break;
            case 3 :
                maxCheck = animalStats.MaxCleanliness;
                break;
        }
        
        if ((stat/maxCheck) * 100f <= 30f)
        {
            return false;
        }

        return true;
    }

    private static float Clamp(float number)
    {
        return Math.Clamp(number, 0f, 120f);
    }
}
