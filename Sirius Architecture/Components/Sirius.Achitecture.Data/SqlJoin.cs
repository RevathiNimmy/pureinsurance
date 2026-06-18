using System;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// A table join clause for use in building a dynamic SQL statement.
    /// </summary>
    public class SqlJoin : ICloneable {

        #region Public Properties

        /// <summary>
        /// Table source expression.
        /// </summary>
        public string TableSource { get; set; }

        /// <summary>
        /// Join keyword, e.g. <c>inner</c>, <c>left outer</c>, <c>right outer</c>, <c>full outer</c>, <c>cross</c>.
        /// </summary>
        public string JoinKeyword { get; set; }

        /// <summary>
        /// Join predicate.
        /// </summary>
        public string JoinPredicate { get; set; }

        /// <summary>
        /// Are the join properties populated?
        /// </summary>
        public bool HasJoin {
            get {
                var has1 = !string.IsNullOrEmpty(this.JoinKeyword);
                var has2 = !string.IsNullOrEmpty(this.JoinPredicate);
                if(has1 ^ has2) {
                    throw new InvalidOperationException(Properties.Resources.ErrorSqlBuilderBadJoinValues);
                }
                return has1;
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public SqlJoin Clone() {
            return new SqlJoin {
                TableSource = this.TableSource,
                JoinKeyword = this.JoinKeyword,
                JoinPredicate = this.JoinPredicate,
            };
        }

        #endregion
    }
}
