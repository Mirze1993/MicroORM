using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM
{
    public class Result<T> 
    {
        public T Value { get; set; } = default;
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public Result<T> SuccessResult(T result)
        {
            Value = result;
            return this;
        }
        public Result<T> ErrorResult(string msg)
        {
            Success = false;
            Message = msg;
            return this;
        }


    }
    public class Result
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }

        public Result()
        {

        }


        public Result ErrorResult(string msg)
        {
            Message = msg;
            Success = false;
            return this;
        }

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public Result SuccessResult(string message)
        {
            Message = message;
            return this;
        }

    }
}
