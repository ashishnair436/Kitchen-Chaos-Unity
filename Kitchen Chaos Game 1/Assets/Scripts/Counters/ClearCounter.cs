using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ClearCounter : BaseCounter 
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject in the counter
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //Player does not carry anything 
            }
        }
        else
        {
            //there is some kitchen object in the counter

            if (player.HasKitchenObject())
            {
                //player is carrying something
            }
            else
            {
                //player does not have anything yet
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
