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

                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                         GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //Player is not carrying a plate but something else.
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter is holding a plate
                       if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                       {
                            player.GetKitchenObject().DestroySelf();
                       }
                    }
                }

            }
            else
            {
                //player does not have anything yet
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
