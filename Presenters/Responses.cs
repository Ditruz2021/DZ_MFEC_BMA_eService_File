// using System.Security.Cryptography.X509Certificates;
// using dotnet_starter.Services.Authorizations;
// using dotnet_starter.Services.Auths;
// using Microsoft.AspNetCore.Mvc;

// namespace dotnet_starter.Presenters
// {
//     public class Result<T>
//     {
//         public bool IsSuccess { get; set; }
//         public T? Data { get; set; }
//         public string? Error { get; set; }

//         public Result() { }

//         private Result(bool isSuccess, T? data, string? error)
//         {
//             IsSuccess = isSuccess;
//             Data = data;
//             Error = error;
//         }

//         public static Result<T> Success(T data) => new Result<T>(true, data, null);
//         public static Result<T> Fail(string error) => new Result<T>(false, default, error);
//     }


//     public class ResultAuth<T1, T2>
//     {
//         public bool IsSuccess { get; }
//         public T1? Data { get; }
//         public T2? Token { get; }
//         public string? Error { get; }

//         private ResultAuth(bool isSuccess, T1? data, T2? token, string? error)
//         {
//             IsSuccess = isSuccess;
//             Data = data;
//             Token = token;
//             Error = error;
//         }

//         public static ResultAuth<T1, T2> Success(T1 data, T2 token)
//             => new ResultAuth<T1, T2>(true, data, token, null);

//         public static ResultAuth<T1, T2> Fail(string error)
//             => new ResultAuth<T1, T2>(false, default, default, error);
//     }



//     public class ResSuccess<T>
//     {
//         public bool Status { get; set; }
//         public string? RequestId { get; set; }
//         public string? ServerTime { get; set; }
//         public T? Data { get; set; }
//     }

//     public class ResSuccessApi<T>
//     {
//         public T? Data { get; set; }
//     }

//     public class ResPaginateSuccess<T>
//     {
//         public bool Status { get; set; }
//         public string? RequestId { get; set; }
//         public int CurrentPage { get; set; }
//         public int PageSize { get; set; }
//         public int TotalPages { get; set; }
//         public int TotalRows { get; set; }
//         public T? Optional { get; set; }
//         public T? Data { get; set; }
//     }
//     public class ResSuccessAuth<T>
//     {
//         public bool Status { get; set; }
//         public string? RequestId { get; set; }
//         public string? ServerTime { get; set; }

//         public T? Data { get; set; }
//         public string? AccessToken { get; set; }
//     }
//     public class ResError<T>
//     {
//         public bool Status { get; set; }
//         public string? RequestId { get; set; }
//         public string? ServerTime { get; set; }

//         public T? Message { get; set; }
//         public string? ErrorType { get; set; }
//     }


//     public class ResponseSuccess<T> : ResSuccess<T>
//     {
//         public static ResponseSuccess<T> Success(T data, HttpContext httpContext)
//         {
//             return new ResponseSuccess<T>
//             {
//                 Status = true,
//                 RequestId = httpContext.TraceIdentifier.Replace(":", ""),
//                 ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
//                 Data = data
//             };
//         }

//         internal static object? Success(Result<AuthResponse> res, HttpContext httpContext)
//         {
//             throw new NotImplementedException();
//         }
//     }

//     public class ResponsePaginateSuccess<T> : ResPaginateSuccess<T>
//     {
//         public static ResponsePaginateSuccess<T> Success(T data, T options, int currentPage, int pageSize, int totalPages, int totalRows, HttpContext httpContext)
//         {
//             return new ResponsePaginateSuccess<T>
//             {
//                 Status = true,
//                 RequestId = httpContext.TraceIdentifier.Replace(":", ""),
//                 CurrentPage = currentPage,
//                 PageSize = pageSize,
//                 TotalPages = totalPages,
//                 TotalRows = totalRows,
//                 Optional = options,
//                 Data = data
//             };
//         }
//     }



//     public class ResponseError<T> : ResError<T>
//     {
//         public static ResponseError<string> Error(string errorMessage, HttpContext httpContext)
//         {
//             return new ResponseError<string>
//             {
//                 Status = false,
//                 RequestId = httpContext.TraceIdentifier.Replace(":", ""),
//                 ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
//                 ErrorType = "error",
//                 Message = errorMessage
//             };
//         }

//         internal static object? Error(object value, HttpContext httpContext)
//         {
//             throw new NotImplementedException();
//         }
//     }


//     public class ResponseValidate<T> : ResError<T>
//     {
//         public static ResponseValidate<object> Error(object validationErrors, HttpContext httpContext)
//         {
//             return new ResponseValidate<object>
//             {
//                 Status = false,
//                 RequestId = httpContext.TraceIdentifier.Replace(":", ""),
//                 ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
//                 ErrorType = "validation",
//                 Message = validationErrors
//             };
//         }
//     }


//     public static class ValidationResponseHandler
//     {
//         public static IActionResult HandleValidationError(ActionContext context)
//         {
//             var firstError = context.ModelState
//                 .Where(x => x.Value?.Errors.Any() == true)
//                 .SelectMany(x => x.Value!.Errors)
//                 .Select(x => x.ErrorMessage)
//                 .FirstOrDefault();

//             return new BadRequestObjectResult(new
//             {
//                 Status = false,
//                 RequestId = context.HttpContext.TraceIdentifier.Replace(":", ""),
//                 ErrorType = "validation",
//                 Message = firstError ?? "Unknown validation error"
//             });
//         }
//     }
// }
using Microsoft.AspNetCore.Mvc;

namespace dotnet_starter.Presenters
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        public Result() { }

        private Result(bool isSuccess, T? data, string? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static Result<T> Success(T data) => new Result<T>(true, data, null);
        public static Result<T> Fail(string error) => new Result<T>(false, default, error);
    }


    public class ResultAuth<T1, T2>
    {
        public bool IsSuccess { get; }
        public T1? Data { get; }
        public T2? Token { get; }
        public string? Error { get; }

        private ResultAuth(bool isSuccess, T1? data, T2? token, string? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Token = token;
            Error = error;
        }

        public static ResultAuth<T1, T2> Success(T1 data, T2 token)
            => new ResultAuth<T1, T2>(true, data, token, null);

        public static ResultAuth<T1, T2> Fail(string error)
            => new ResultAuth<T1, T2>(false, default, default, error);
    }



    public class ResSuccess<T>
    {
        public bool Status { get; set; }
        public string? RequestId { get; set; }
        public string? ServerTime { get; set; }
        public T? Data { get; set; }
    }

    public class ResSuccessApi<T>
    {
        public T? Data { get; set; }
    }

    public class ResPaginateSuccess<T>
    {
        public bool Status { get; set; }
        public string? RequestId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
        public T? Data { get; set; }
    }
    public class ResSuccessAuth<T>
    {
        public bool Status { get; set; }
        public string? RequestId { get; set; }
        public string? ServerTime { get; set; }

        public T? Data { get; set; }
        public string? AccessToken { get; set; }
    }
    public class ResError<T>
    {
        public bool Status { get; set; }
        public string? RequestId { get; set; }
        public string? ServerTime { get; set; }

        public T? Message { get; set; }
        public string? ErrorType { get; set; }
    }


    public class ResponseSuccess<T> : ResSuccess<T>
    {
        public static ResponseSuccess<T> Success(T data, HttpContext httpContext)
        {
            return new ResponseSuccess<T>
            {
                Status = true,
                RequestId = httpContext.TraceIdentifier.Replace(":", ""),
                ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Data = data
            };
        }
    }
    public class ResponsePaginateSuccess<T> : ResPaginateSuccess<T>
    {
        public static ResponsePaginateSuccess<T> Success(T data, int currentPage, int pageSize, int totalPages, int totalRows, HttpContext httpContext)
        {
            return new ResponsePaginateSuccess<T>
            {
                Status = true,
                RequestId = httpContext.TraceIdentifier.Replace(":", ""),
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRows = totalRows,
                Data = data
            };
        }
    }
    public class ResponseSuccessAuth<T> : ResSuccessAuth<T>
    {

        public static ResponseSuccessAuth<T> Success(T data, string accessToken, HttpContext httpContext)
        {
            return new ResponseSuccessAuth<T>
            {
                Status = true,
                RequestId = httpContext.TraceIdentifier.Replace(":", ""),
                ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Data = data,
                AccessToken = accessToken,
            };
        }
    }
    public class ResponseError<T> : ResError<T>
    {
        public static ResponseError<string> Error(string errorMessage, HttpContext httpContext)
        {
            return new ResponseError<string>
            {
                Status = false,
                RequestId = httpContext.TraceIdentifier.Replace(":", ""),
                ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ErrorType = "error",
                Message = errorMessage
            };
        }

        internal static object? Error(object value, HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }


    public class ResponseValidate<T> : ResError<T>
    {
        public static ResponseValidate<object> Error(object validationErrors, HttpContext httpContext)
        {
            return new ResponseValidate<object>
            {
                Status = false,
                RequestId = httpContext.TraceIdentifier.Replace(":", ""),
                ServerTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ErrorType = "validation",
                Message = validationErrors
            };
        }
    }


    public static class ValidationResponseHandler
    {
        public static IActionResult HandleValidationError(ActionContext context)
        {
            var firstError = context.ModelState
                .Where(x => x.Value?.Errors.Any() == true)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .FirstOrDefault();

            return new BadRequestObjectResult(new
            {
                Status = false,
                RequestId = context.HttpContext.TraceIdentifier.Replace(":", ""),
                ErrorType = "validation",
                Message = firstError ?? "Unknown validation error"
            });
        }
    }
}
