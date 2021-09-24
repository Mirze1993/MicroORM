namespace MicroORM.Mediator
{
    public partial class Mediator
    {
        public interface IRequestHandler<in X, out Y> where X : IRequest where Y : IResponse
        {
            Y Handler(X x);
        }
    }
}
