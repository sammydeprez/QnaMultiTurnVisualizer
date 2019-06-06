using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnaMakerVisualization.Models
{
    public class VisualTreeItem
    {
        public VisualTreeItemText Text { get; set; }
        public List<VisualTreeItem> Children { get; set; }
    }
    public class VisualTreeItemText
    {
        public string PromptQuestion { get; set; }
        public string Answer { get; set; }
        public long QnaId { get; set; }
    }
}
