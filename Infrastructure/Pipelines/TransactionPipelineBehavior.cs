using System;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Infrastructure.Configurations;
using MediatR;
using Serilog;

namespace Infrastructure.Pipelines
{
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest :
        ITransactionalRequest<TResponse>
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionPipelineBehavior(ApplicationDbContext dbContext
            )
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {

                var currentTransaction = _dbContext.Database.CurrentTransaction;
                var isFirstTransaction = currentTransaction == null;
                if (isFirstTransaction)
                {
                    Log.Information($"Begin transaction {typeof(TRequest).Name}");
                    await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                }

                var response = await next();

                if (isFirstTransaction)
                {
                    await _dbContext.Database.CommitTransactionAsync(cancellationToken);

                    Log.Information($"Committed transaction {typeof(TRequest).Name}");
                }


                return response;
            }
            catch (Exception ex)
            {


                var isTransactionExist = _dbContext.Database.CurrentTransaction != null;

                if (isTransactionExist)
                {
                    Log.Information($"Rollback transaction executed {typeof(TRequest).Name}");
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);

                    //Log.Error(ex.Message, ex.StackTrace);
                    var errorId = Guid.NewGuid();
                    Log.ForContext("Type", "Error").ForContext("StackTrace", ex.StackTrace)
                        .ForContext("Exception", ex, true)
                        .Error(ex, ex.Message + ". {@errorId}", errorId);
                }


                throw;
            }
        }
    }
}
