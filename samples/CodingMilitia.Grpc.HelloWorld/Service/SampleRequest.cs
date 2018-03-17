namespace CodingMilitia.Grpc.HelloWorld.Service
{
    [Bond.Schema]
    class SampleRequest
    {
        [Bond.Id(0)]
        public int Value { get; set; }
    }
}