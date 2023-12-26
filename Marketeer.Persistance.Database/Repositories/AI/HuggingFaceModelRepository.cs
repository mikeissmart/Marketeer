using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Persistance.Database.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.AI
{
    public interface IHuggingFaceModelRepository : IRepository<HuggingFaceModel>
    {
        Task<List<HuggingFaceModel>> GetHuggingFaceModelsAsync();
        Task<HuggingFaceModel?> GetHuggingFaceModelByIdAsync(int id);
        Task<HuggingFaceModel?> GetDefaultHuggingFaceModelAsync();
    }

    public class HuggingFaceModelRepository : BaseRepository<HuggingFaceModel>, IHuggingFaceModelRepository
    {
        public HuggingFaceModelRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }

        public async Task<List<HuggingFaceModel>> GetHuggingFaceModelsAsync() =>
            await GetAsync();

        public async Task<HuggingFaceModel?> GetHuggingFaceModelByIdAsync(int id) =>
            await GetSingleOrDefaultAsync(x => x.Id == id);

        public async Task<HuggingFaceModel?> GetDefaultHuggingFaceModelAsync() =>
            await GetSingleOrDefaultAsync(x => x.IsDefault);
    }
}
