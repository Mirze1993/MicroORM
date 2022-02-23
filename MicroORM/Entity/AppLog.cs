using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM.Entity
{
    public class AppLog
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string Text { get; set; }
    }
}
