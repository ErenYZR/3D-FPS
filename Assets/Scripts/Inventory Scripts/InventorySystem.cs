using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem 
{
	[SerializeField] private List<InventorySlot> inventorySlots;
	public List<InventorySlot> InventorySlots => inventorySlots;
	public int InventorySize => InventorySlots.Count;

	public UnityAction<InventorySlot> OnInventorySlotChanged;

	public InventorySystem(int size)//Slot sayýsýný ayarlayan constructor.
	{
		inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
			inventorySlots.Add(new InventorySlot());
        }
    }

	public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
	{
		if(ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Check whether item exist in inventory.
		{
            foreach (var slot in invSlot)
            {
				if (slot.EnoughRoomLeftInStack(amountToAdd))
				{
					slot.AddToStack(amountToAdd);
					OnInventorySlotChanged?.Invoke(slot);
					return true;
				}
            }
		}
				
		if(HasFreeSlot(out InventorySlot freeSlot))// Gets the first available slot.
		{
			if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
			{			
				freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
				OnInventorySlotChanged?.Invoke(freeSlot);
				return true;
			}
		}

		return false;
	}
	public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
	{
		invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();

		return invSlot == null ? false : true;
	}

	public bool HasFreeSlot(out InventorySlot freeSlot)
	{
		freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
		return freeSlot == null ? false : true;
	}
}
