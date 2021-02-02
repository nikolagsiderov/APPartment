namespace APPartment.Data.Enums
{
    // These values MUST be unique and in sync with APPartment.Data.DataAccessContext ObjectType table values
    public enum ObjectTypes
    {
        User = 1,
        Home = 2,
        HomeStatus = 3, // sub-object
        HomeSettings = 4, // sub-object
        Inventory = 5,
        Hygiene = 6,
        Issue  = 7,
        Message = 8,
        Comment = 9, // sub-object
        Image = 10, // sub-object
        Survey = 12,
        Chore = 13,
        HomeUser = 14,
        Audit = 15
    }
}
