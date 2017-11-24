using CommonAPI.Data;
using ExpressSearch.Data;
using ExpressSearch.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressSearch.Repository
{
    /// <summary>
    /// 快递查询
    /// </summary>
    public class ExpressSearchRepository: RepositoryBase<ExpressSearch.Models.SysExpressSearch>, IExpressSearchRepository
    {

    }
}
