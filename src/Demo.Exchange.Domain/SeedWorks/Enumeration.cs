namespace Demo.Exchange.Domain.SeedWorks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class Enumeration
    {
        protected Enumeration(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }
    }
}