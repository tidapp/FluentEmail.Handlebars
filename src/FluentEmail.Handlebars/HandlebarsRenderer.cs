using System;
using System.IO;
using System.Threading.Tasks;

using FluentEmail.Core.Interfaces;

using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;

namespace FluentEmail.Handlebars
{
    public class HandlebarsRenderer : ITemplateRenderer
    {
        private readonly IHandlebars _engine;

        public HandlebarsRenderer()
        {
            _engine = HandlebarsDotNet.Handlebars.Create();
            HandlebarsHelpers.Register(_engine);
        }

        public HandlebarsRenderer(string templateRoot)
            : this()
        {
            RegisterTemplatesFrom(templateRoot);
        }

        public async Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
        {
            var compiledTemplate = _engine.Compile(template);

            var result = compiledTemplate(model);

            return await Task.FromResult(result);
        }

        string ITemplateRenderer.Parse<T>(string template, T model, bool isHtml)
        {
            return ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
        }

        private void RegisterTemplatesFrom(string templateRoot)
        {
            if (!Directory.Exists(templateRoot))
            {
                throw new ArgumentException($"The templateRoot directory {templateRoot} does not exist");
            }

            var templates = Directory.GetFiles(templateRoot, "*.html.hbs");

            foreach (var template in templates)
            {
                var name = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(template)).ToLower();
                var content = File.ReadAllText(template);
                _engine.RegisterTemplate(name, content);
            }
        }
    }
}
