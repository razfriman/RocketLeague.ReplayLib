using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RocketLeague.ReplayLib
{
    public class ClassNetCache
    {
        public int ObjectIndex { get; private set; }
        public int ParentId { get; private set; }
        public int Id { get; private set; }
        public int PropertiesLength { get; private set; }
        public IDictionary<int, ClassNetCacheProperty> Properties { get; private set; }
        public List<ClassNetCache> Children { get; private set; }

        public ClassNetCache Parent { get; set; }
        public bool Root;

        public static ClassNetCache Deserialize(BinaryReader br)
        {
            var classNetCache = new ClassNetCache
            {
                ObjectIndex = br.ReadInt32(),
                ParentId = br.ReadInt32(),
                Id = br.ReadInt32(),
                Children = new List<ClassNetCache>(),
                PropertiesLength = br.ReadInt32(),
                Properties = new Dictionary<int, ClassNetCacheProperty>()
            };

            for (var i = 0; i < classNetCache.PropertiesLength; ++i)
            {
                var prop = ClassNetCacheProperty.Deserialize(br);
                classNetCache.Properties[prop.Id] = prop;
            }

            return classNetCache;
        }

        public IEnumerable<ClassNetCacheProperty> AllProperties
        {
            get
            {
                foreach (var prop in Properties.Values)
                {
                    yield return prop;
                }

                if (Parent != null)
                {
                    foreach (var prop in Parent.AllProperties)
                    {
                        yield return prop;
                    }
                }
            }
        }

        private int? _maxPropertyId;

        public int MaxPropertyId
        {
            get
            {
                _maxPropertyId ??= AllProperties.Max(x => x.Id);
                return _maxPropertyId.Value;
            }
        }

        public ClassNetCacheProperty GetProperty(int id)
        {
            return Properties.TryGetValue(id, out var property) 
                ? property 
                : Parent?.GetProperty(id);
        }
    }
}