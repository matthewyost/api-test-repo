namespace Api.Dtos.Paycheck
{
	public class PaycheckDto
	{
		public Guid EmployeeId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime DateOfBirth { get; set; }

		public decimal NetPay { get; set; }

	}
}
