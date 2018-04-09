using CodingMilitia.Grpc.Serializers;
using Xunit;
using grpc = global::Grpc.Core;

namespace CodingMilitia.Grpc.Tests.Serializers
{
    //Yeah... composition over inheritance and stuff... 
    //It was just a lot easier and faster to do it this way in this case.
    public abstract class BaseSerializerTests
    {
        private readonly ISerializer _serializer;

        public BaseSerializerTests(ISerializer serializer)
        {
            _serializer = serializer;
        }

        [Fact]
        public void GivenASimpleMessageTheCustomSerializerOutputMatchesTheDefaultOne()
        {
            //given
            var defaultSerializer = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::CodingMilitia.Grpc.Tests.Generated.SampleRequest.Parser.ParseFrom);
            var defaultMessage = new CodingMilitia.Grpc.Tests.Generated.SampleRequest { Value = 1 };
            var customMessage = new Service.SampleRequest { Value = 1 };

            //when
            var defaultSerizationResult = defaultSerializer.Serializer(defaultMessage);
            var customSerializationResult = _serializer.ToBytes(customMessage);

            //then
            Assert.Equal(defaultSerizationResult, customSerializationResult);
        }

        [Fact]
        public void GivenASimpleMessageTheCustomSerializerCanDeserializeTheResultOfTheDefaultOne()
        {
            //given
            var defaultSerializer = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::CodingMilitia.Grpc.Tests.Generated.SampleRequest.Parser.ParseFrom);
            var defaultMessage = new CodingMilitia.Grpc.Tests.Generated.SampleRequest { Value = 1 };

            //when
            var defaultSerizationResult = defaultSerializer.Serializer(defaultMessage);
            var customDeserializerResult = _serializer.FromBytes<Service.SampleRequest>(defaultSerizationResult);

            //then
            Assert.NotNull(customDeserializerResult);
            Assert.Equal(defaultMessage.Value, customDeserializerResult.Value);
        }
    }
}