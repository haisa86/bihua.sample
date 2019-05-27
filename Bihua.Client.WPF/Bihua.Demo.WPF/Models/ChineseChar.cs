using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bihua.Demo.WPF.Models
{
    public class ChineseChar
    {
        /// <summary>
        /// 获取或设置汉字的部首
        /// </summary>
        public string BuShou { get; set; }

        /// <summary>
        /// 获取或设置笔画图形的画布原始尺寸大小，单位像素
        /// </summary>
        public int RectSize { get; set; }

        /// <summary>
        /// 获取或设置汉字的拼音集合
        /// </summary>
        public string[] Pinyins { get; set; }

        /// <summary>
        /// 获取或设置全部笔画图形的点
        /// </summary>
        public IEnumerable<IEnumerable<int[]>> BiHuas { get; set; }

        /// <summary>
        /// 获取或设置全部笔顺图形的点
        /// </summary>
        public IEnumerable<IEnumerable<int[]>> BiShuns { get; set; }
    }
}
