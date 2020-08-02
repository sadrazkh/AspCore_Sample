using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Utilities;

namespace WebFramework.Api
{
    public class ResultViewModel : object
    {
        public ResultViewModel() : base()
        {
            ErrorMessages =
                new System.Collections.Generic.List<string>();

            HiddenMessages =
                new System.Collections.Generic.List<string>();

            InformationMessages =
                new System.Collections.Generic.List<string>();
        }

        // **********
        public bool Succeeded { get; set; }
        // **********

        // **********
        public System.Collections.Generic.IList<string> ErrorMessages { get; protected set; }
        // **********

        // **********
        public System.Collections.Generic.IList<string> HiddenMessages { get; protected set; }
        // **********

        // **********
        public System.Collections.Generic.IList<string> InformationMessages { get; protected set; }
        // **********

        public void AddErrorMessage(string message)
        {
            message =
                message.Fix();

            if (message.Length == 0)
            {
                return;
            }

            if (ErrorMessages.Contains(message))
            {
                return;
            }

            ErrorMessages.Add(message);
        }

        public void AddHiddenMessage(string message)
        {
            message =
                message.Fix();

            if (message.Length == 0)
            {
                return;
            }

            if (HiddenMessages.Contains(message))
            {
                return;
            }

            HiddenMessages.Add(message);
        }

        public void AddInformationMessage(string message)
        {
            message =
                message.Fix();

            if (message.Length == 0)
            {
                return;
            }

            if (InformationMessages.Contains(message))
            {
                return;
            }

            InformationMessages.Add(message);
        }

        public void ClearAllMessages()
        {
            ClearErrorMessages();
            ClearHiddenMessages();
            ClearInformationMessages();
        }

        public void ClearNonHiddenMessages()
        {
            ClearErrorMessages();
            ClearInformationMessages();
        }

        public void ClearErrorMessages()
        {
            ErrorMessages.Clear();
        }

        public void ClearHiddenMessages()
        {
            ErrorMessages.Clear();
        }

        public void ClearInformationMessages()
        {
            InformationMessages.Clear();
        }
    }

    public class ApiResult : ResultViewModel
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null) : base()
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplay();
        }

        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value?.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound);
        }
        #endregion
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData Data { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = null)
            : base(isSuccess, statusCode, message)
        {
            Data = data;
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, null);
        }

        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);
        }

        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            var message = result.Value?.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, null, result.Content);
        }

        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, null);
        }

        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, (TData)result.Value);
        }
        #endregion
    }
}
