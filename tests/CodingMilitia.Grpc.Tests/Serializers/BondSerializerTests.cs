using CodingMilitia.Grpc.Serializers;
using Xunit;
using grpc = global::Grpc.Core;

namespace CodingMilitia.Grpc.Tests.Serializers
{
    public class BondSerializerTests : BaseSerializerTests
    {
        public BondSerializerTests() : base(new BondSerializer())
        {

        }
    }
}