namespace Syncfusion.Core.Extensions
{
    /// <summary>
    /// Class BoolExtensions.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// BOOL 扩展
        /// </summary>
        /// <param name="b">Bool 数据</param>
        /// <returns>Int</returns>
        public static int ToInt(this bool b)
        {
            if (b)
                return 1;
            else
                return 0;
        }
    }
}
