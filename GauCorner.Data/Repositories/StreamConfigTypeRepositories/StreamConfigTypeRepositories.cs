using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
namespace GauCorner.Data.Repositories.StreamConfigTypeRepositories
{

    public class StreamConfigTypeRepositories : GenericRepositories<StreamConfigType>, IStreamConfigTypeRepositories
    {
        public StreamConfigTypeRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}