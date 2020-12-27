using System.Collections.Generic;
using System;
using DataAnalyse;

namespace services{
    public interface IPersonAnalyse
    {
        public List<int> getTimespanByPersonInTimeDuring();
        public List<Decimal> getTelCostByPerson();
        public List<int> getTelTimesByCityInTimeDuring();
        
    }
}