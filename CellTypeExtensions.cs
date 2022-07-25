namespace TestingTest
{
    public static class CellTypeExtensions
    {
        public static char GetCharRepresentation(this CellType cellType)
        {
            return cellType switch
            {
                CellType.Player => '@',
                CellType.Enemy => '8',
                CellType.Corpse => 'X',
                CellType.Floor => '.',
                _ => '#',
            };
        }
    }
}