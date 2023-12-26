using AutoMapper;
using Marketeer.Core.Domain.Dtos.AI;
using Marketeer.Persistance.Database.Repositories.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Service.AI
{
    public interface IHuggingFaceService : ICoreService
    {
        Task<HuggingFaceModelDto> GetDefaultHuggingFaceModelAsync();
        Task<List<HuggingFaceModelDto>> GetHuggingFaceModelsAsync();
    }

    public class HuggingFaceService : BaseCoreService, IHuggingFaceService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HuggingFaceService> _logger;
        private readonly IHuggingFaceModelRepository _huggingFaceModelRepository;

        public HuggingFaceService(IMapper mapper, ILogger<HuggingFaceService> logger, IHuggingFaceModelRepository huggingFaceModelRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _huggingFaceModelRepository = huggingFaceModelRepository;
        }

        public async Task<HuggingFaceModelDto> GetDefaultHuggingFaceModelAsync()
        {
            try
            {
                var model = await _huggingFaceModelRepository.GetDefaultHuggingFaceModelAsync();
                if (model == null)
                    throw new ArgumentNullException("No Default HuggingFaceModel");

                return _mapper.Map<HuggingFaceModelDto>(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<HuggingFaceModelDto>> GetHuggingFaceModelsAsync()
        {
            try
            {
                return _mapper.Map<List<HuggingFaceModelDto>>(await _huggingFaceModelRepository.GetHuggingFaceModelsAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
