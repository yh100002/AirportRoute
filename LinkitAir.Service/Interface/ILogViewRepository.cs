using System.Collections.Generic;
using LinkitAir.Service.Dto;

namespace LinkitAir.Service.Interface
{
    public interface ILogViewRepository
    {
        double? Total();
        int CountOnLevel(string value);  

        LogAggregatedValueDto AggregatedValuesForAll();

        Dictionary<string, int> GetErrorSet();
    }
}