using Api.Dtos.Paycheck;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	public class GetPaycheckQuery : IRequest<ApiResponse<PaycheckDto>>
	{
		public GetPaycheckQuery(Guid employeeId)
		{
			EmployeeId = employeeId;
		}

		public Guid EmployeeId { get; set; }
	}
}
