using System.IO;
using ProtoBuf;

namespace CodingMilitia.Grpc.Serializers
{
    public class ProtoBufSerializer : ISerializer
    {
        public byte[] ToBytes<T>(T input)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, input);
                return stream.ToArray();
            }
        }

        public T FromBytes<T>(byte[] input)
        {
            using(var stream = new MemoryStream(input))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }
}