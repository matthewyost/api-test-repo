
using Api.Models;
using System.Text.Json;

namespace Api.Middleware
{
	/// <summary>
	/// Middleware for handling any unknown exceptions that may be thrown by the application.
	/// 
	/// This will ensure that we always return a valid JSON response to the client even
	/// if the application throws an exception.
	/// </summary>
	public class ExceptionMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly JsonSerializerOptions _options = new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};

		/// <summary>
		/// Constructor for <see cref="ExceptionMiddleware"/>
		/// </summary>
		/// <param name="logger"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		/// <summary>
		/// Handles the exception by logging it and returning a JSON response to the client.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="ex"></param>
		/// <returns></returns>
		private async Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			context.Response.ContentType = "application/json";
			var response = context.Response;

			var errorResponse = new ApiResponse<object>
			{
				Success = false,
				Error = ex.Message
			};

			_logger.LogError(ex, ex.Message);

			await JsonSerializer.SerializeAsync(response.Body, errorResponse, errorResponse.GetType(), _options);
		}
	}
}
