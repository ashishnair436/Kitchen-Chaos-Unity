using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;



    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;


    private float spawnPlateTimer;
    [SerializeField] private float spawnPlateTimerMax = 4f;

    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 5;

    private void Update()
    {

        spawnPlateTimer += Time.deltaTime;
        
        if(spawnPlateTimer>= spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;


            if(plateSpawnedAmount < plateSpawnedAmountMax)
            {
                plateSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is emptyHanded
            if (plateSpawnedAmount > 0)
            {
                //There is atleast one plate here
                plateSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
