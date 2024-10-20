using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
	[SerializeField] MouseItemData mouseInventoryItem;
	protected InventorySystem inventorySystem;
	protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
	public InventorySystem InventorySystem => inventorySystem;
	public Dictionary<InventorySlot_UI,InventorySlot> SlotDictionary => slotDictionary;

	protected virtual void Start()
	{

	}

	public abstract void AssignSlot(InventorySystem invToDisplay);

	protected virtual void UpdateSlot(InventorySlot updatedSlot)
	{
        foreach (var slot in slotDictionary)
        {
            if(slot.Value == updatedSlot) // Slot value - the "under the hood" inventory slot. 
			{
				slot.Key.UpdateUISlot(updatedSlot); // Slot key - UI representation of the value.
			}
        }
    }

	public void SlotClicked(InventorySlot_UI clickedUISlot)
	{
		bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

		//Slottaki eþyayý elimize alma
		if(clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
		{
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))//yýðýný bölme)
            {
				mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
				clickedUISlot.UpdateUISlot();
				return;
            }
            else
            {
				mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
				clickedUISlot.ClearSlot();
				return;
			}
        }
		//Elimizdeki eþyayý slota koyma
		if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
		{
			clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
			clickedUISlot.UpdateUISlot();

			mouseInventoryItem.ClearSlot();
			return;
		}
		//Elimizde ve slotta eþya varsa...
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
			bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;//Mousela elimizde ayný eþya mý var ?

			if (isSameItem	&& clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
			{ //Eþyalar ayný ise ve toplamýnda yýðýn dolmuyorsa
				clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
				clickedUISlot.UpdateUISlot();

				mouseInventoryItem.ClearSlot();
				return;
			}
			else if(isSameItem &&
				!clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
				if (leftInStack < 1) SwapSlots(clickedUISlot); //Yýðýn dolu olduðu için eþyalarý takaslýyoruz.
                else //Yýðýn dolu olmadýðý için mouse'dan gerektiði kadar eþya alýyoruz.
                {
					int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

					clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
					clickedUISlot.UpdateUISlot();

					var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
					mouseInventoryItem.ClearSlot();
					mouseInventoryItem.UpdateMouseSlot(newItem);
					return;
                }
            }
            else if (!isSameItem)
            {
				SwapSlots(clickedUISlot);
				return;
            }
        }
    }

	private void SwapSlots(InventorySlot_UI clickedUISlot)
	{
		var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
		mouseInventoryItem.ClearSlot();

		mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
		clickedUISlot.ClearSlot();

		clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
		clickedUISlot.UpdateUISlot();
	}
}
