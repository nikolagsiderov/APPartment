namespace APPartment.Enums
{
    // These values MUST be unique and in sync with APPartment.Data.DataAccessContext ObjectType table values
    public enum ObjectTypes
    {
        User = 1,
        House = 2,
        HouseStatus = 3, // sub-object
        HouseSettings = 4, // sub-object
        Inventory = 5,
        Hygiene = 6,
        Issue  = 7,
        Message = 8,
        Comment = 9, // sub-object
        Image = 10, // sub-object
        History = 11, // sub-object
        Survey = 12
    }
}
