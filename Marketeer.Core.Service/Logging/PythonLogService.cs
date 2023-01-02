using AutoMapper;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Core.Service.Logging
{
    public interface IPythonLogService : ICoreService
    {
    }

    public class PythonLogService : IPythonLogService, ICoreService
    {
        private readonly IMapper _mapper;
        private readonly IPythonLogRepository _pythonLogRepository;

        public PythonLogService(IMapper mapper,
            IPythonLogRepository pythonLogRepository)
        {
            _mapper = mapper;
            _pythonLogRepository = pythonLogRepository;
        }
    }
}
