using MediatR;
using ProductService.Application.Core.Abstractions.Data;
using ProductService.Application.Core.Extensions;

namespace ProductService.Application.Core.Behaviours
{
    /// <summary>
    /// Represents the unit of work behaviour middleware.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    internal sealed class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkBehaviour{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UnitOfWorkBehaviour(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,  RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = await next();

            if (request.IsQuery())
            {
                return response;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
