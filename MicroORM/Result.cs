using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM
{
    public class Result<T> 
    {
        public T t { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public Result<T> SuccessResult(T result)
        {
            t = result;
            return this;
        }
    }
}
