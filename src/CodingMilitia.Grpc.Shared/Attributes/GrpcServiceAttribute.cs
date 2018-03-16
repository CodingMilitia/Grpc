using System;

namespace CodingMilitia.Grpc.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)] 
    public class GrpcServiceAttribute : Attribute
    {
        public string Name { get; set; }

        public GrpcServiceAttribute(string name)
        {
            Name = name;
        }
    }
}