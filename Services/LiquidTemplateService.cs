using DotLiquid;
using Lombiq.LiquidMarkup.Models;
using Lombiq.LiquidMarkup.Services.Filters;
using Lombiq.LiquidMarkup.Services.Tags;
using Orchard;
using Orchard.Caching.Services;
using Orchard.DisplayManagement.Shapes;

namespace Lombiq.LiquidMarkup.Services
{
    public class LiquidTemplateService : ILiquidTemplateService
    {
        private static bool _templateIsConfigured;

        private readonly IWorkContextAccessor _wca;
        private readonly ICacheService _cacheService;


        public LiquidTemplateService(IWorkContextAccessor wca, ICacheService cacheService)
        {
            _wca = wca;
            _cacheService = cacheService;
        }
        
    
        public string ExecuteTemplate(string liquidSource, dynamic model)
        {
            EnsureTemplateConfigured();

            model.WorkContext = _wca.GetContext();
            var templateModel = new StaticShape(model);

            var liquidTemplate = _cacheService.Get(liquidSource, () => Template.Parse(liquidSource));
            return liquidTemplate.Render(new RenderParameters
            {
                LocalVariables = Hash.FromAnonymousObject(new { Model = templateModel }),
                RethrowErrors = true
            });
        }

        public void VerifySource(string liquidSource)
        {
            EnsureTemplateConfigured();

            Template.Parse(liquidSource);
        }


        // This method potentially runs from multiple threads, also the first time but this is safe to do so.
        private static void EnsureTemplateConfigured()
        {
            if (_templateIsConfigured) return;

            // Currently only global configuration is possible, see: https://github.com/formosatek/dotliquid/issues/93
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();

            Template.RegisterSafeType(
                typeof(ShapeMetadata),
                new[] { "Type", "DisplayType", "Position", "PlacementSource", "Prefix", "Wrappers", "Alternates", "WasExecuted" });
            
            // Tags:
            // Note that both Liquid-style all lowercase and C#-style CamelCase names are made available.
            Template.RegisterTag<AddClassToCurrentShapeTag>("addclasstocurrentshape");
            Template.RegisterTag<AddClassToCurrentShapeTag>("AddClassToCurrentShape");
            Template.RegisterTag<AntiForgeryTokenOrchardTag>("antiforgerytokenorchard");
            Template.RegisterTag<AntiForgeryTokenOrchardTag>("AntiForgeryTokenOrchard");
            Template.RegisterTag<AntiForgeryTokenValueOrchardTag>("antiforgerytokenvalueorchard");
            Template.RegisterTag<AntiForgeryTokenValueOrchardTag>("AntiForgeryTokenValueOrchard");
            Template.RegisterTag<AddPageClassNamesTag>("addpageclassnames");
            Template.RegisterTag<AddPageClassNamesTag>("AddPageClassNames");
            Template.RegisterTag<ClassForPageTag>("classforpage");
            Template.RegisterTag<ClassForPageTag>("ClassForPage");
            Template.RegisterTag<DisplayTag>("display");
            Template.RegisterTag<DisplayTag>("Display");
            Template.RegisterTag<PageTitleTag>("pagetitle");
            Template.RegisterTag<PageTitleTag>("PageTitle");
            Template.RegisterTag<RegisterLinkTag>("registerlink");
            Template.RegisterTag<RegisterLinkTag>("RegisterLink");
            Template.RegisterTag<ScriptTag>("script");
            Template.RegisterTag<ScriptTag>("Script");
            Template.RegisterTag<ScriptTag>("scriptrequire");
            Template.RegisterTag<ScriptTag>("ScriptRequire");
            Template.RegisterTag<StyleTag>("style");
            Template.RegisterTag<StyleTag>("Style");
            Template.RegisterTag<StyleTag>("stylerequire");
            Template.RegisterTag<StyleTag>("StyleRequire");
            Template.RegisterTag<SetMetaTag>("setmeta");
            Template.RegisterTag<SetMetaTag>("SetMeta");
            Template.RegisterTag<HrefTag>("href");
            Template.RegisterTag<HrefTag>("Href");

            // Filters:
            Template.RegisterFilter(typeof(DisplayFilter));

            _templateIsConfigured = true;
        }
    }
}