using System;
namespace DIdemo
{
    public interface ITravel
    {
        /// <summary>
        /// 获取旅游产品线路列表
        /// </summary>
        /// <returns>The line list.</returns>
        string GetLineList();
    }
}
