namespace CodingMilitia.Grpc.Tests.Service
{
    [ProtoBuf.ProtoContract]
    [Bond.Schema]
    public class SampleResponse
    {
        [ProtoBuf.ProtoMember(1)]
        [Bond.Id(0)]
        public int Value { get; set; }
    }
}