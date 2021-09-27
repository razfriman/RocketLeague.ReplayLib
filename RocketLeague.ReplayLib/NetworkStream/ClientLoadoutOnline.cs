using System.Collections.Generic;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ClientLoadoutOnline
    {
        public List<List<ProductAttribute>> ProductAttributeLists { get; private set; }

        public static ClientLoadoutOnline Deserialize(BitReader br, uint engineVersion, uint licenseeVersion,
            string[] objectNames)
        {
            var clo = new ClientLoadoutOnline
            {
                ProductAttributeLists = new List<List<ProductAttribute>>()
            };

            var listCount = br.ReadByte();
            for (var i = 0; i < listCount; ++i)
            {
                var productAttributes = new List<ProductAttribute>();

                var productAttributeCount = br.ReadByte();
                for (var j = 0; j < productAttributeCount; ++j)
                {
                    productAttributes.Add(ProductAttribute.Deserialize(br, engineVersion, licenseeVersion,
                        objectNames));
                }

                clo.ProductAttributeLists.Add(productAttributes);
            }

            return clo;
        }
    }
}