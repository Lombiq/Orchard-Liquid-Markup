using Orchard;
using Orchard.DisplayManagement.Implementation;

namespace Lombiq.LiquidMarkup.Services
{
    /// <summary>
    /// Injects the WorkContext into shapes so its properties are accessible from Liquid templates too.
    /// </summary>
    public class WorkContextShapeInjector : IShapeDisplayEvents
    {
        private readonly IWorkContextAccessor _wca;


        public WorkContextShapeInjector(IWorkContextAccessor wca)
        {
            _wca = wca;
        }
        
        
        public void Displaying(ShapeDisplayingContext context)
        {
            context.Shape.WorkContext = _wca.GetContext();
        }

        public void Displayed(ShapeDisplayedContext context)
        {
        }
    }
}