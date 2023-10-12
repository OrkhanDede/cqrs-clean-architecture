using AutoWrapper.Wrappers;
using Core.Exceptions;
using Core.Resources;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Services
{
    public class ExceptionService
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public ExceptionService(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
        }

        public ApiException Error(string errorCode, int statusCode = 400)
        {
            var message = _localizer[errorCode];
            return new ApiException(message, statusCode, errorCode);
        }

        public WrongRequestException WrongRequestException()
        {
            const string key = ResourceKey.WrongRequest;
            var message = _localizer[key];
            return new WrongRequestException(message, key);
        }
        public ApiException InvalidToken()
        {
            const string key = ResourceKey.InvalidToken;
            return Error(key);
        }
        public ApiException CantImpersonateYourself()
        {
            const string key = ResourceKey.CanImpersonateYourself;
            return Error(key);
        }

        public RecordNotFoundException RecordNotFoundException(string key = ResourceKey.RecordNotFound)
        {
            var message = _localizer[key];
            return new RecordNotFoundException(message, key);
        }

        public RecordNotEditableException RecordNotEditableException()
        {
            const string key = ResourceKey.RecordNotEditable;
            var message = _localizer[key];
            return new RecordNotEditableException(message, key);
        }

        public RecordAlreadyExistException RecordAlreadyExistException()
        {
            const string key = ResourceKey.RecordAlreadyExist;
            var message = _localizer[key];
            return new RecordAlreadyExistException(message, key);
        }

        public LockedUserException LockedUserException()
        {
            const string key = ResourceKey.LockedUser;
            var message = _localizer[key];
            return new LockedUserException(message, key);
        }

        public InvalidSignInException InvalidSignInException()
        {
            const string key = ResourceKey.InvalidSignIn;
            var message = _localizer[key];
            return new InvalidSignInException(message, key);
        }

        public InternalServerException InternalServerException()
        {
            const string key = ResourceKey.InternalServer;
            var message = _localizer[key];
            return new InternalServerException(message, key);
        }

        public InsufficientTableAccessException InsufficientTableAccessException()
        {
            const string key = ResourceKey.InsufficientTableAccess;
            var message = _localizer[key];
            return new InsufficientTableAccessException(message, key);
        }

        public IdMustBeGuidException IdMustBeGuidException()
        {
            const string key = ResourceKey.IdMustBeGuid;
            var message = _localizer[key];
            return new IdMustBeGuidException(message, key);
        }

    }
}
