using FluentEmail.Core.Interfaces;
using FluentEmail.Handlebars;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FluentEmailHandlebarsBuilderExtensions
    {
        public static FluentEmailServicesBuilder AddHandlebarsRenderer(this FluentEmailServicesBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, HandlebarsRenderer>());
            return builder;
        }

        /// <summary>
        /// Automatically loads templates from the specified directory
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="templateRoot"></param>
        /// <returns></returns>
        public static FluentEmailServicesBuilder AddHandlebarsRenderer(this FluentEmailServicesBuilder builder, string templateRoot)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, HandlebarsRenderer>(s => new HandlebarsRenderer(templateRoot)));
            return builder;
        }
    }
}
