using Core.Enums;
using Domain.Common.Configurations;

namespace Infrastructure.Services
{
    public class AccessLimitService
    {
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;
        public AccessLimitService(AuthService authService,ExceptionService exceptionService)
        {
            _authService = authService;
         
            _exceptionService = exceptionService;
        }


        public bool CheckCanModifyRecord(Entity entity, bool throwError = true)
        {
            if (entity == null) return false;

            if (entity.Status != RecordStatusEnum.Active)
            {
                if (throwError)
                    throw _exceptionService.RecordNotEditableException();
                else
                    return false;
            }



            return true;
        }
  

    }
}
