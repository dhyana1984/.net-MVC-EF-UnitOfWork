using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Configuration
{
    public class ManifestTokenResolver : IManifestTokenResolver
    {
        private readonly IManifestTokenResolver _defaultResolver = new DefaultManifestTokenResolver();

        public string ResolveManifestToken(DbConnection connection)
        {
            if (connection is SqlConnection sqlConn)
            {
                return @"2012";
            }
            else
            {
                return _defaultResolver.ResolveManifestToken(connection);
            }
        }
    }
}
