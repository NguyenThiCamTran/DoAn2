using System.Collections.Generic;

namespace CuaHangTapHoas.ViewModels
{
    public class ChartViewModel
    {
        public List<string> Labels { get; internal set; }
        public List<double> Data { get; internal set; }
    }
}