
using System.Linq;
using UnityEngine.Events;

public class KeyItem : InventoryItem, IUsable
{
    public bool IsUsable()
    {
        var doors = Door.AllDoors.Where(x => x.KeyToOpen == this && x.IsInPlayerRange());
        if (doors.Count() == 0) return false;
        return doors.Any(x => x.isLocked);
    }

    public void Use()
    {
        var doors = Door.AllDoors.Where(x => x.KeyToOpen == this && x.IsInPlayerRange());
        if (doors.Count() == 0) return;
        foreach (var door in doors)
        {
            if (door.isLocked)
            {
                door.Unlock();
            }
        }
        if (Door.AllDoors.Count(x => x.KeyToOpen == this && x.isLocked) == 0) Amount = 0;
    }
}
