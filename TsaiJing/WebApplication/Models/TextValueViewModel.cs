using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class TextValueViewModel<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
        public T Parent { get; set; }
    }
}
