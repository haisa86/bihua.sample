using Bihua.Demo.WPF.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Bihua.Demo.WPF
{
    /// <summary>
    /// 汉字笔画笔顺播放控件
    /// </summary>
    public partial class BihuaActor : UserControl
    {
        //用于保存笔顺动画故事面板
        List<Storyboard> _biShunStoryboardList = new List<Storyboard>();
        //当前正在播放的笔顺索引
        int _biShunPlayingIndex = -1;

        public ChineseChar ChineseChar
        {
            get { return (ChineseChar)GetValue(ChineseCharProperty); }
            set { SetValue(ChineseCharProperty, value); }
        }

        public static readonly DependencyProperty ChineseCharProperty =
            DependencyProperty.Register("ChineseChar", typeof(ChineseChar), typeof(BihuaActor), new PropertyMetadata(null, (d, e) =>
             {
                 BihuaActor self = d as BihuaActor;
                 self.Fill(e.NewValue as ChineseChar);
             }));

        public BihuaActor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="chineseChar"></param>
        void Fill(ChineseChar chineseChar)
        {
            RemoveStoryBoards();
            gdBiHuas.Children.Clear();
            gdBiShuns.Children.Clear();
            if (chineseChar == null) return;

            FillBihuas(chineseChar);
            FillBiShuns(chineseChar);
        }

        /// <summary>
        /// 填充笔画
        /// </summary>
        /// <param name="chineseChar"></param>
        void FillBihuas(ChineseChar chineseChar)
        {
            if (chineseChar.BiHuas == null) return;

            foreach (Tuple<Geometry, double> tuple in GetGeometries(chineseChar.BiHuas, gdBiHuas.ActualWidth / chineseChar.RectSize))
            {
                Path path = new Path
                {
                    Data = tuple.Item1,
                    Fill = Brushes.Gray,
                };
                gdBiHuas.Children.Add(path);
            }
        }

        /// <summary>
        /// 填充笔顺并播放动画
        /// </summary>
        /// <param name="chineseChar"></param>
        void FillBiShuns(ChineseChar chineseChar)
        {
            if (chineseChar.BiShuns == null) return;

            foreach (Tuple<Geometry, double> tuple in GetGeometries(chineseChar.BiShuns, gdBiShuns.ActualWidth / chineseChar.RectSize))
            {
                Path path = new Path
                {
                    Data = tuple.Item1,
                    Stroke = Brushes.Green,
                    StrokeLineJoin = PenLineJoin.Round,
                    StrokeThickness = 2
                };
                path.StrokeDashArray = new DoubleCollection(new double[] { tuple.Item2 / path.StrokeThickness });
                path.StrokeDashOffset = tuple.Item2 / path.StrokeThickness;
                gdBiShuns.Children.Add(path);

                Storyboard sb = new Storyboard();
                sb.Completed += Sb_Completed;
                DoubleAnimation da = new DoubleAnimation();
                da.From = path.StrokeDashOffset;
                da.To = 0;
                da.Duration = TimeSpan.FromMilliseconds(Math.Min(1500, tuple.Item2));
                Storyboard.SetTarget(da, path);
                Storyboard.SetTargetProperty(da, new PropertyPath(Path.StrokeDashOffsetProperty));
                sb.Children.Add(da);

                _biShunStoryboardList.Add(sb);
            }

            _biShunStoryboardList[_biShunPlayingIndex = 0].Begin(gdBiShuns.Children[_biShunPlayingIndex] as Path, true);
        }

        void RemoveStoryBoards()
        {
            for (int i = 0; i < _biShunStoryboardList.Count; ++i)
            {
                _biShunStoryboardList[i].Completed -= Sb_Completed;
                _biShunStoryboardList[i].Remove(gdBiShuns.Children[i] as Path);
            }
            _biShunStoryboardList.Clear();
        }

        void Sb_Completed(object sender, EventArgs e)
        {
            _biShunPlayingIndex++;
            if (_biShunPlayingIndex > _biShunStoryboardList.Count - 1)
            {
                for (int i = 0; i < _biShunStoryboardList.Count; ++i)
                {
                    _biShunStoryboardList[i].Stop(gdBiShuns.Children[i] as Path);
                }

                foreach (Path path in gdBiShuns.Children)
                {
                    path.StrokeDashOffset = path.StrokeDashArray[0];
                }

                _biShunPlayingIndex = 0;
            }
            _biShunStoryboardList[_biShunPlayingIndex].Begin(gdBiShuns.Children[_biShunPlayingIndex] as Path, true);
        }

        double Distance(int[] p0, int[] p1, double ratio)
        {
            return Math.Sqrt((Math.Pow((p1[0] - p0[0]), 2) + Math.Pow((p1[1] - p0[1]), 2))) * ratio;
        }

        IEnumerable<Tuple<Geometry, double>> GetGeometries(IEnumerable<IEnumerable<int[]>> data, double ratio)
        {
            foreach (List<int[]> yibi in data)
            {
                StreamGeometry geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    double distance = 0;
                    ctx.BeginFigure(new Point(yibi[0][0] * ratio, yibi[0][1] * ratio), true, false);
                    for (int i = 1; i < yibi.Count; ++i)
                    {
                        ctx.LineTo(new Point(yibi[i][0] * ratio, yibi[i][1] * ratio), true, true);
                        distance += Distance(yibi[i - 1], yibi[i], ratio);
                    }
                    yield return new Tuple<Geometry, double>(geometry, distance);
                }
            }
        }

    }
}
