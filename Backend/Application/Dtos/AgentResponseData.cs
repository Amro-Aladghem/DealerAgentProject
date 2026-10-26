using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public class AgentResponseData
    {
        public List<CarDto>? Cars { get; set; }
        public List<CarAccessoryDto>? CarAccessories { get; set; }
    }
}
