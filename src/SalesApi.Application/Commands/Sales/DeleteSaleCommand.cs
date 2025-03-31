using MediatR;

namespace Application.Commands.Sales
{
    public class DeleteSaleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSaleCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}


