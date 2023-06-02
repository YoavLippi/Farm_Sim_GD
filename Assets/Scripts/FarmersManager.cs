using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class FarmersManager : MonoBehaviour
{
    [SerializeField] private FarmerHandler[] farmers;

    [SerializeField] private AnimalHandler[] animals;

    [SerializeField] private int currentFarmer;

    [SerializeField] private bool canSwitch;

    [SerializeField] private Camera main;

    public bool CanSwitch
    {
        get => canSwitch;
        set => canSwitch = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        canSwitch = true;
        if (farmers[0])
        {
            farmers[0].setSelected(true);
            currentFarmer = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.mouseScrollDelta.magnitude != 0 || Input.GetKeyDown(KeyCode.E))
        {
            if (canSwitch)
            {
                ChangeSelected();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !farmers[currentFarmer].Occupied && farmers[currentFarmer].Active)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.zero,
                Mathf.Infinity, LayerMask.GetMask("Farmers"));
            if (hit && hit.collider.gameObject != farmers[currentFarmer].gameObject)
            {
                
            }
            else
            {
                Vector3 position = main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 1;
                farmers[currentFarmer].MoveToPos(new Vector2(position.x, position.y));
            }
        }
    }

    public void StartTask(string c)
    {
        careType care = (careType) Enum.Parse( typeof(careType), c);
        StartCoroutine(DoCare(currentFarmer, care));
    }

    private IEnumerator DoCare(int farmerID, careType c)
    {
        farmers[farmerID].setOccupied(true);
        AnimalHandler ah = farmers[farmerID].CollidedHandler;
        StatBlock currentStats = ah.getStats();
        
        float difference;
        float barDifference;
        switch (c)
        {
            case careType.Attention :
                difference = ah.AnimalStats.MaxAttention - ah.CurrentAttention;
                farmers[farmerID].setTaskbarSize(difference);
                farmers[farmerID].setTaskbarValue(0f);
                
                do
                {
                    yield return new WaitForSeconds(0.2f);
                    currentStats.Attention += 2.5f;
                    barDifference = difference - (ah.AnimalStats.MaxAttention - ah.CurrentAttention);
                    farmers[farmerID].setTaskbarValue(barDifference);
                    if (currentStats.Attention > ah.AnimalStats.MaxAttention)
                    {
                        currentStats.Attention = ah.AnimalStats.MaxAttention;
                    }
                    farmers[farmerID].CollidedHandler.SetStats(currentStats);
                } while (ah.CurrentAttention < ah.AnimalStats.MaxAttention);
                break;
            
            case careType.Cleaning :
                difference = ah.AnimalStats.MaxCleanliness - ah.CurrentCleanliness;
                farmers[farmerID].setTaskbarSize(difference);
                farmers[farmerID].setTaskbarValue(0f);
                
                do
                {
                    yield return new WaitForSeconds(0.2f);
                    currentStats.Cleanliness += 2.5f;
                    barDifference = difference - (ah.AnimalStats.MaxCleanliness - ah.CurrentCleanliness);
                    farmers[farmerID].setTaskbarValue(barDifference);
                    if (currentStats.Cleanliness > ah.AnimalStats.MaxCleanliness)
                    {
                        currentStats.Cleanliness = ah.AnimalStats.MaxCleanliness;
                    }
                    farmers[farmerID].CollidedHandler.SetStats(currentStats);
                } while (ah.CurrentCleanliness < ah.AnimalStats.MaxCleanliness);
                break;
            
            case  careType.Energy :
                difference = ah.AnimalStats.MaxEnergy - ah.CurrentEnergy;
                farmers[farmerID].setTaskbarSize(difference);
                farmers[farmerID].setTaskbarValue(0f);
                
                do
                {
                    yield return new WaitForSeconds(0.2f);
                    currentStats.Energy += 2.5f;
                    barDifference = difference - (ah.AnimalStats.MaxEnergy - ah.CurrentEnergy);
                    farmers[farmerID].setTaskbarValue(barDifference);
                    if (currentStats.Energy > ah.AnimalStats.MaxEnergy)
                    {
                        currentStats.Energy = ah.AnimalStats.MaxEnergy;
                    }
                    farmers[farmerID].CollidedHandler.SetStats(currentStats);
                } while (ah.CurrentEnergy < ah.AnimalStats.MaxEnergy);
                break;
            
            case careType.Food :
                difference = ah.AnimalStats.MaxHunger - ah.CurrentHunger;
                farmers[farmerID].setTaskbarSize(difference);
                farmers[farmerID].setTaskbarValue(0f);
                
                do
                {
                    yield return new WaitForSeconds(0.2f);
                    currentStats.Hunger += 2.5f;
                    barDifference = difference - (ah.AnimalStats.MaxHunger - ah.CurrentHunger);
                    farmers[farmerID].setTaskbarValue(barDifference);
                    if (currentStats.Hunger > ah.AnimalStats.MaxHunger)
                    {
                        currentStats.Hunger = ah.AnimalStats.MaxHunger;
                    }
                    farmers[farmerID].CollidedHandler.SetStats(currentStats);
                } while (ah.CurrentHunger < ah.AnimalStats.MaxHunger);
                break;
        }
        farmers[farmerID].setTaskbarValue(100f);
        yield return new WaitForSeconds(0.2f);
        farmers[farmerID].setOccupied(false);
    }

    public void ChangeSelected()
    {
        bool foundFlag = false;
        if (Input.mouseScrollDelta.y >= 0)
        {
            for (int i = currentFarmer + 1; i < farmers.Length; i++)
            {
                if (farmers[i])
                {
                    if (!farmers[i].Occupied)
                    {
                        farmers[currentFarmer].setSelected(false);
                        farmers[i].setSelected(true);
                        currentFarmer = i;
                        foundFlag = true;
                        break;
                    }
                }
            }

            if (!foundFlag)
            {
                for (int i = 0; i < currentFarmer; i++)
                {
                    if (farmers[i])
                    {
                        if (!farmers[i].Occupied)
                        {
                            farmers[currentFarmer].setSelected(false);
                            farmers[i].setSelected(true);
                            currentFarmer = i;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = currentFarmer - 1; i >= 0; i--)
            {
                if (farmers[i])
                {
                    if (!farmers[i].Occupied)
                    {
                        farmers[currentFarmer].setSelected(false);
                        farmers[i].setSelected(true);
                        currentFarmer = i;
                        foundFlag = true;
                        break;
                    }
                }
            }

            if (!foundFlag)
            {
                for (int i = farmers.Length - 1; i > currentFarmer; i--)
                {
                    if (farmers[i])
                    {
                        if (!farmers[i].Occupied)
                        {
                            farmers[currentFarmer].setSelected(false);
                            farmers[i].setSelected(true);
                            currentFarmer = i;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void ChangeSelectedToSpecific(FarmerHandler newSelection)
    {
        if (newSelection.Occupied || newSelection.isSelected() || !canSwitch) return;
        for (int i = 0; i < farmers.Length; i++)
        {
            if (farmers[i] == newSelection)
            {
                farmers[currentFarmer].setSelected(false);
                farmers[i].setSelected(true);
                currentFarmer = i;
                break;
            }
        }
    }

    public void disableAllFarmers()
    {
        foreach (var farmer in farmers)
        {
            farmer.Active = false;
            farmer.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }

        foreach (var animal in animals)
        {
            animal.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        }
    }

    public void enableAllFarmers()
    {
        foreach (var animal in animals)
        {
            animal.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
        }
        
        foreach (var farmer in farmers)
        {
            farmer.Active = true;
            farmer.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        }
    }
}