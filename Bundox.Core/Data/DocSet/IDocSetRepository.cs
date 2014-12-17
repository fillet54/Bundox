using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundox.Core.Data.DocSet
{
    public interface IDocSetRepository
    {
        IEnumerable<DocSetEntity> GetAll();
        DocSetEntity GetEntityById(int id);
    }
}
