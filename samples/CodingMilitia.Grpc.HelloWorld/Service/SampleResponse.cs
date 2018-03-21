namespace CodingMilitia.Grpc.HelloWorld.Service
{
    [Bond.Schema]
    public class SampleResponse
    {
        [Bond.Id(0)]
        public int Value { get; set; }
    }
}