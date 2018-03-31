using System;

namespace CodingMilitia.Grpc.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)] 
    public class GrpcMethodAttribute : Attribute
    {
        public string Name { get; set; }

        public GrpcMethodAttribute(string name)
        {
            Name = name;
        }
    }
}