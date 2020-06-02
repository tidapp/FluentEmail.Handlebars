![.NET Core](https://github.com/matthewturner/FluentEmail.Handlebars/workflows/.NET%20Core/badge.svg)

![alt text](https://github.com/lukencode/FluentEmail/raw/master/assets/fluentemail_logo_64x64.png "FluentEmail")

# FluentEmail Handlebars Plugin

Allows you to use Handlebars rendering engine and author your emails in Handlebars, using the excellent [Handlebars.Net](https://github.com/rexm/Handlebars.Net).

**Basic Usage**

```csharp
var email = Email
    	.From("john@email.com")
    	.To("bob@email.com", "bob")
    	.Subject("hows it going bob")
    	.Body("yo dawg, sup?")
		.Send();
```


**Dependency Injection**

You can configure FluentEmail in startup.cs with these helper methods. This will by default inject IFluentEmail (send a single email) and IFluentEmailFactory (used to send multiple emails in a single context) with the 
ISender and ITemplateRenderer configured using AddHandlebarsRenderer(), AddSmtpSender() or other packages.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	services
		.AddFluentEmail("fromemail@test.test")
		.AddHandlebarsRenderer()
		.AddSmtpSender("localhost", 25);
}
```

**Using a template**

```csharp
// Using Handlebars templating package (or set using AddHandlebarsRenderer in services)
Email.DefaultRenderer = new HandlebarsRenderer();

var template = "Dear {{Name}}, You are totally {{Compliment}}.";

var email = Email
    .From("bob@hotmail.com")
    .To("somedude@gmail.com")
    .Subject("woo nuget")
    .UsingTemplate(template, new { Name = "Luke", Compliment = "Awesome" });


```

**Using a template with optional partial templates**

Registers partial templates described fully [here](https://github.com/rexm/Handlebars.Net#registering-partials)

```csharp
// Using Handlebars templating package (or set using AddHandlebarsRenderer(templatePath) in services)
Email.DefaultRenderer = new HandlebarsRenderer("some path to templates");
```