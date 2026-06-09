namespace Quinela.Application.Common.Results
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("Un Result exitoso no puede tener error.");
            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("Un Result fallido requiere un error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(value, true, Error.None);
        public static Result<T> Failure<T>(Error error) => new(default, false, error);
    }

    public sealed class Result<T> : Result
    {
        private readonly T? _value;

        internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("No se puede leer Value de un Result fallido.");
    }
}
