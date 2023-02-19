using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerDelistReasonRepository : IRepository<TickerDelistReason>
    {

    }

    public class TickerDelistReasonRepository : BaseRepository<TickerDelistReason>, ITickerDelistReasonRepository
    {
        public TickerDelistReasonRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
