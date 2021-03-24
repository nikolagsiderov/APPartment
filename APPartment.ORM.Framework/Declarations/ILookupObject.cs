namespace APPartment.ORM.Framework.Declarations
{
    public interface ILookupObject
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }
    }
}
