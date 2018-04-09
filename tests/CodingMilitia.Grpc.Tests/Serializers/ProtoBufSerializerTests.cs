using CodingMilitia.Grpc.Serializers;
using Xunit;
using grpc = global::Grpc.Core;

namespace CodingMilitia.Grpc.Tests.Serializers
{
    public class ProtoBufSerializerTests : BaseSerializerTests
    {
        public ProtoBufSerializerTests() : base(new ProtoBufSerializer())
        {

        }
    }
}