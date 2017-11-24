using CommonAPI.Data;
using ExpressSearch.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressSearch.domain
{
    /// <summary>
    /// 快递查询
    /// </summary>
    public interface IExpressSearchRepository : IRepositoryBase<ExpressSearch.Models.SysExpressSearch>
    {
    }
}
