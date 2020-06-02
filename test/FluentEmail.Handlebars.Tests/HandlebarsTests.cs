using FluentEmail.Core;
using FluentEmail.Handlebars;

using NUnit.Framework;
using System;
using System.Dynamic;
using System.IO;

namespace FluentEmail.Handlebars.Tests
{
    public class HandlebarsTests
    {
        const string toEmail = "bob@test.com";
        const string fromEmail = "johno@test.com";
        const string subject = "sup dawg";

        [SetUp]
        public void SetUp()
        {
            Email.DefaultRenderer = new HandlebarsRenderer();
        }

        [Test]
        public void Anonymous_Model_With_List_Template_Matches()
        {
            string template = "sup {{Name}} here is a list {{#each Numbers }}{{this}}{{/each}}";

            var email = Email
                .From(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });

            Assert.AreEqual("sup LUKE here is a list 123", email.Data.Body);
        }

        [Test]
        public void New_Anonymous_Model_Template_Matches()
        {
            string template = "sup {{Name}}";

            var email = new Email(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "LUKE" });

            Assert.AreEqual("sup LUKE", email.Data.Body);
        }

        [Test]
        public void New_Anonymous_Model_With_List_Template_Matches()
        {
            string template = "sup {{Name}} here is a list {{#each Numbers }}{{this}}{{/each}}";

            var email = new Email(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });

            Assert.AreEqual("sup LUKE here is a list 123", email.Data.Body);
        }

        [Test]
        public void New_Reuse_Cached_Templates()
        {
            string template = "sup {{Name}} here is a list {{#each Numbers }}{{this}}{{/each}}";
            string template2 = "sup {{Name}} this is the second template";

            for (var i = 0; i < 10; i++)
            {
                var email = new Email(fromEmail)
                    .To(toEmail)
                    .Subject(subject)
                    .UsingTemplate(template, new { Name = i, Numbers = new string[] { "1", "2", "3" } });

                Assert.AreEqual("sup " + i + " here is a list 123", email.Data.Body);

                var email2 = new Email(fromEmail)
                    .To(toEmail)
                    .Subject(subject)
                    .UsingTemplate(template2, new { Name = i });

                Assert.AreEqual("sup " + i + " this is the second template", email2.Data.Body);
            }
        }

        [Test]
        public void Should_be_able_to_use_named_templates()
        {
            var templateRoot = Directory.GetCurrentDirectory();
            Email.DefaultRenderer = new HandlebarsRenderer(templateRoot);

            string template = @"sup {{Name}} here is a list {{#each Numbers }}{{> strong}}{{/each}}";

            var email = new Email(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "LUKE", Numbers = new string[] { "1", "2" } });

            Assert.AreEqual($"sup LUKE here is a list <strong>1</strong><strong>2</strong>", email.Data.Body);
        }
    }
}
