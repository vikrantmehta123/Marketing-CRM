
using System;

namespace Metaforge_Marketing.Models
{
    public class Remark
    {

        public Customer Customer { get; set; }
        public DateTime EventDate { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }

    }
}
