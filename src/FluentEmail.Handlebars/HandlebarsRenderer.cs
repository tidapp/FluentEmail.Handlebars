using System;
using System.IO;
using System.Threading.Tasks;

using FluentEmail.Core.Interfaces;

namespace FluentEmail.Handlebars
{
    public class HandlebarsRenderer : ITemplateRenderer
    {
        private readonly string _templateRoot;

        public HandlebarsRenderer()
        {
        }

        public HandlebarsRenderer(string templateRoot)
        {
            if (!Directory.Exists(templateRoot))
            {
                throw new ArgumentException($"The templateRoot directory {templateRoot} does not exist");
            }
            _templateRoot = templateRoot;
        }

        public async Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
        {
            RegisterTemplatesIfRequired();

            var compiledTemplate = HandlebarsDotNet.Handlebars.Compile(template);

            var result = compiledTemplate(model);

            return await Task.FromResult(result);
        }

        string ITemplateRenderer.Parse<T>(string template, T model, bool isHtml)
        {
            return ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
        }

        private void RegisterTemplatesIfRequired()
        {
            if (string.IsNullOrEmpty(_templateRoot))
            {
                return;
            }

            var templates = Directory.GetFiles(_templateRoot, "*.html");

            foreach (var template in templates)
            {
                var name = Path.GetFileNameWithoutExtension(template).ToLower();
                var content = File.ReadAllText(template);
                HandlebarsDotNet.Handlebars.RegisterTemplate(name, content);
            }
        }
    }
}
