using System.Collections.Generic;
using System.Linq;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Extension methods applied to a collection of table sources.
    /// </summary>
    public static class SqlJoinListHelper {

        /// <summary>
        /// Determines whether the specified table source is in the collection, regardless of what join predicate it may have.
        /// </summary>
        public static bool Contains(this IEnumerable<SqlJoin> source, string tableSource) {
            return source.Any(j => j.TableSource == tableSource);
        }

        /// <summary>
        /// Determines whether the specified table source is in the collection, regardless of what join predicate it may have.
        /// </summary>
        public static bool Contains(this IEnumerable<SqlJoin> source, string tableSource, IEqualityComparer<string> comparer) {
            if(comparer == null) {
                comparer = EqualityComparer<string>.Default;
            }
            return source.Any(j => comparer.Equals(j.TableSource, tableSource));
        }
    }
}
