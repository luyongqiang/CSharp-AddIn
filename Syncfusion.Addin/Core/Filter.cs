using System.Collections.Generic;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Class Filter.
    /// </summary>
    public class Filter : IFilter
    {
        /// <summary>
        /// The filter string
        /// </summary>
        private string filterString;

        public string FilterString
        {
            get
            {
                return filterString;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public Filter(string filter)
        {
            this.filterString = filter;
        }

        #region IFilter Members

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>Bool</returns>
        public bool Match(IServiceReference reference)
        {
            return true;
        }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns>Bool</returns>
        public bool Match(Dictionary<string, object> dictionary)
        {
            return true;
        }

        /// <summary>
        /// 匹配  区分大小写
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns>Bool</returns>
        public bool MatchCase(Dictionary<string, object> dictionary)
        {
            return true;
        }

        #endregion IFilter Members
    }
}