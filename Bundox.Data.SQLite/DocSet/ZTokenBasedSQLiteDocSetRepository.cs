using Bundox.Core.Data.DocSet;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.Linq;

namespace Bundox.Data.SQLite.DocSet
{
    public class ZTokenBasedSQLiteDocSetRepository : SQLiteDocSetRepositoryBase
    {
        public ZTokenBasedSQLiteDocSetRepository(string dbPath)
            :base(dbPath)
        {
        }

        public override IEnumerable<DocSetEntity> GetAll()
        {
            IEnumerable<DocSetEntity> results;
            using (var connection = new SQLiteConnection(_ConnectionString))
            {
                using (var context = new DataContext(connection))
                {
                    var tokens = context.GetTable<TokenEntity>();
                    results = GetDocSetEntitiesFromTokenEntities(context, tokens)
                              .ToList();
                }
            }
            return results;
        }

        public override DocSetEntity GetEntityById(int id)
        {
            DocSetEntity result;
            using (var connection = new SQLiteConnection(_ConnectionString))
            {
                using (var context = new DataContext(connection))
                {
                    var tokens = context.GetTable<TokenEntity>()
                                 .Where(token => token.Id == id);
                    result = GetDocSetEntitiesFromTokenEntities(context, tokens)
                             .ToList() // First does not work with the SQLite SQL Sytax
                             .FirstOrDefault();
                }
            }
            return result;
        }

        private IQueryable<DocSetEntity> GetDocSetEntitiesFromTokenEntities(DataContext context, IQueryable<TokenEntity> tokenQuery)
        {
            return tokenQuery
                   .Join(context.GetTable<MetaInformationEntity>(),
                         token => token.MetaInformationId,
                         meta => meta.Id,
                         (token, meta) => new { token, meta })
                   .Join(context.GetTable<FilePathEntity>(),
                         others => others.meta.FilePathId,
                         filepath => filepath.Id,
                         (others, filepath) => new { others.meta, others.token, filepath })
                   .Join(context.GetTable<TokenTypeEntity>(),
                         others => others.token.Type,
                         tokenType => tokenType.ID,
                         (others, tokenType) => new { others.meta, others.token, others.filepath, tokenType })
                   .Select(e => ZTokenStructureToDocSetEntity(e.token, e.filepath, e.meta.Anchor, e.tokenType));
        }

        private DocSetEntity ZTokenStructureToDocSetEntity(TokenEntity token, FilePathEntity filepath, string anchor, TokenTypeEntity tokenType)
        {
            return new DocSetEntity
            {
                ID = token.Id,
                Name = token.Name,
                Type = tokenType.Name,
                Path = filepath.Path + "#" + anchor
            };
        }
    }
}
