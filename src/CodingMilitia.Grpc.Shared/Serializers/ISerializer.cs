namespace CodingMilitia.Grpc.Serializers
{
    public interface ISerializer
    {
         byte[] ToBytes<T>(T input);
         T FromBytes<T>(byte[] input);
    }
}