namespace CodingMilitia.Grpc.HelloWorld.Service
{
    [Bond.Schema]
    public class SampleRequest
    {
        [Bond.Id(0)]
        public int Value { get; set; }
    }
}