using System;
namespace terraria
{
    public interface ICharacter
    {
        string GetImageFileName();
        int GetDrawingPriority();
        Wish GetWish(int x, int y, Game game);
        bool DeadInConflict(ICharacter conflictedObject, Game game);
        bool IsBrokenBy(IInventoryItem weapon, Game game);
    }
}
