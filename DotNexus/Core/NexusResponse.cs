namespace DotNexus.Core
{
    public class NexusResponse<T>
    {
        public T Result { get; set; }
        public NexusError Error { get; set; }
    }
}