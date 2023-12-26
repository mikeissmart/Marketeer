using Marketeer.Core.Domain.Entities.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.AI
{
    public class HuggingFaceModelDto : EntityDto, IRefactorType,
        IMapFrom<HuggingFaceModel>,
        IMapTo<HuggingFaceModel>
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
