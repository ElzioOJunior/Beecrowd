using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Sales
{
    public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly IRepository<Sale> _repository;

        public DeleteSaleCommandHandler(IRepository<Sale> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdWithIncludesAsync(request.Id, sale=> sale.Items);
            if (sale == null)
            {
                throw new CustomException(
                            type: "InvalidData",
                            message: "Sale not found",
                            detail: $"The sale with ID {request.Id} was not found in the system."
                        );
            }

            sale.Canceled = true;

            if (sale.Items != null && sale.Items.Any())
            {
                foreach (var item in sale.Items)
                {
                    item.Canceled = true;
                }
            }

            await _repository.UpdateAsync(sale);
            return true;
        }
    }
}
