namespace NCheck.Test
{
    using System.Collections.Generic;

    public class Parent : IIdentifiable
    {
        private readonly Dictionary<string, string> indexer;

        public Parent()
        {
            indexer = new Dictionary<string, string>();
        }

        public int Id { get; set; }

        object IIdentifiable.Id
        {
            get { return Id; }
        }

        public string Name { get; set; }

        public string this[string test]
        {
            get { return indexer[test]; }
            set { indexer[test] = value; }
        }

        public int Another { get; set; }

        public Child Favourite { get; set; }

        public List<Child> Children { get; set; }
    }
}
