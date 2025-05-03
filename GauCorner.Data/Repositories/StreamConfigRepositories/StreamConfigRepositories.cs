using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.StreamConfigRepositories
{
    public class StreamConfigRepositories : GenericRepositories<StreamConfig>, IStreamConfigRepositories
    {
        public StreamConfigRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}