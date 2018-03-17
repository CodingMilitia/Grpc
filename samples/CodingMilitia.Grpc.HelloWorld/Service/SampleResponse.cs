namespace CodingMilitia.Grpc.HelloWorld.Service
{
    [Bond.Schema]
    class SampleResponse
    {
        [Bond.Id(0)]
        public int Value { get; set; }
    }
}