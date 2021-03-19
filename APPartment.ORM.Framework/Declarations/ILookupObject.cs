namespace APPartment.ORM.Framework.Declarations
{
    public interface ILookupObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }
    }
}
