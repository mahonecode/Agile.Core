using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Core
{
    /// <summary>
    /// API统一返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AgileReturn<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 返回具体数据
        /// </summary>
        public T data { get; set; }
    }
}
