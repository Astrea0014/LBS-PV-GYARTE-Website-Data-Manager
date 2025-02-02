using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.DbUtil
{
    internal enum DbAction
    {
        INSERT,
        UPDATE,
        DELETE
    }
    internal interface IDbAction
    {
        public abstract DbAction Action { get; }
    }
    internal class DbInsertAction : IDbAction
    {
        public DbAction Action => DbAction.INSERT;
    }
    internal class DbUpdateAction : IDbAction
    {
        public DbAction Action => DbAction.UPDATE;
    }
}
