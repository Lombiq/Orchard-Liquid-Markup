# Orchard Liquid Markup Readme



## Project Description

Orchard module for adding support for templates written in Liquid Markup (http://liquidmarkup.org/). Uses DotLiquid (http://dotliquidmarkup.org/).


## Documentation

### Overview

This module adds the ability to use shape templates written in [Liquid Markup](http://liquidmarkup.org/) and uses the [DotLiquid library](http://dotliquidmarkup.org/). [See what Liquid offers](https://github.com/Shopify/liquid/wiki/Liquid-for-Designers). The module is also available for [DotNest](http://dotnest.com/) sites for templating.

**Naming conventions are C#-style!** This means that all the properties you can access on viewmodels from your templates will have the same names as usual but built-in Liquid filters (like `upcase`) will also behave in the same way (i.e. you'll be able to use it as `Upcase`).

There are the following features in the module:

- Liquid Markup: doesn't provide any user-accessible features, just basic services.
- Liquid Markup Templates: extends Orchard.Templates so you can write Liquid templates from the admin.
- Liquid Markup View Engine: adds a view engine that enables you to use .liquid Liquid-formatted templates in your themes and modules. You can use this to develop Liquid templates quickly from an IDE.

Note that since Liquid was designed to be safe you can't call arbitrary methods on services that may be accessible from templates.

### Examples

Do note the following:

- Although presented here with C#-style notation, custom tags are usable all lowercase too (tags are conventionally all lowercase in Liquid). E.g. these are both valid: `{% Display User %}` and `{% display User %}`.
- While strings are wrapped in double quotes here single quotes (`'`) work equally.
- When passing parameters to tags you can not just pass simple strings but also variable references. E.g. these will both work: `{% Display User, Parameter1: "some value" %}` and  `{% Display User, Parameter1: Model.WorkContext.CurrentUser.UserName %}`.

#### Accessing the model

	Accessing shape properties:
	{{ Model.Id }}
	{{ Model.Metadata.Type }}
	
	Accessing the viewmodel from the admin User shape:
	{{ Model.CurrentUser.UserName }}
	{{ Model.CurrentUser.Email }}
	
	Dynamic expressions on ContentItems work, e.g. you can do this from the Content shape or content part shapes:
	{{ Model.ContentItem.TitlePart.Title }}

	Accessing array or list elements work as well. E.g. if you add a MediaLibraryPickerField on the Page content type with the name Images you'll be able to access the attached Image items (given that there are at least two) like following:
	{{ Model.ContentItem.Page.Images.MediaParts[0].MediaUrl } <- First image.
	{{ Model.ContentItem.Page.Images.MediaParts[1].MediaUrl } <- Second image.
	{{ Model.ContentItem.Page.Images.Ids[0] }} <-- ID of the first image.
	{{ Model.ContentItem.Page.Images.Ids[1] }} <-- ID of the second image.

#### Accessing the WorkContext

The properties on the WorkContext (and the properties of those objects) are also accessible:

	Accessing the currently authenticated user's name: 
	{% if Model.WorkContext.CurrentUser != null %}
	  {{ Model.WorkContext.CurrentUser.UserName }}
	{% else %}
	  Please log in!
	{% endif %}
	
	Using the HttpContext:
	{{ Model.WorkContext.HttpContext.Request.Url.AbsoluteUri }}

#### Including static resources

	Including stylesheets and scripts:
	{% Style "/url/to/stylesheet.css" %}
	You can omit the single quotes or use double quotes instead if you wish.
	Note that relative virtual paths (like "~/Themes/MyTheme/Styles/styles.css") will work too as usual.
	
	{% Script "/url/to/script.js", head %}
	The second parameter is the script location: head or foot. The default is foot so you can omit the parameter if you want to include the script in the footer.
	
	You can also reference resources by their names if they are defined in an enabled feature:
	{% ScriptRequire "jQueryUI", head %}
	{% StyleRequire "jQueryUI_Orchard" %}

#### Working with shapes

	Displaying shapes from the viewmodel with the Display filter, e.g. from the Content shape:
	<article>
	{{ Model.Content | Display }}
	</article>
	Note that there are no quotes around Model.Content!
	
	Displaying any shape with the display tag, here the User shape:
	{% Display User %}
	
	You can also give the shape input parameters as from C#:
	{% Display User, Parameter1: "some value", Parameter2: "some other value" %}
	These then can be uses from inside the User shape as Model.Parameter1 and Model.Parameter2 respectively.

	CSS classes can be added to shapes much like how Model.Classes.Add("pagination"); works in Razor:
	{% AddClassToCurrentShape "pagination" %}

#### Changing global properties of the HTML document

	Adds a <link> tag to the head of the document.
	{% RegisterLink, Condition: "gte IE 7", Href: "https://en.wikipedia.org/static/favicon/wikipedia.ico", Rel: "shortcut icon", Title: "favicon", Type: "image/x-icon" %}
	The same as the following in Razor: 
	RegisterLink(new Orchard.UI.Resources.LinkEntry
		{
			Condition = "gte IE 7",
			Href = "https://en.wikipedia.org/static/favicon/wikipedia.ico",
			Rel = "shortcut icon",
			Title = "favicon",
			Type = "image/x-icon"
		});

	Adds a <meta> tag to the head of the document (or modifies an existing one).
	{% SetMeta, Charset: "utf-8", Content: "Wordpress", HttpEquiv: "X-Generator", Name: "generator" %}
	The same as the following in Razor:
	SetMeta(new Orchard.UI.Resources.MetaEntry
		{
			Charset = "utf-8",
			Content = "Wordpress",
			HttpEquiv = "X-Generator",
			Name = "generator"
		});

	Sets the title of the current page. Equivalent to using Html.Title("Title comes here"); in Razor.
    {% PageTitle "Title comes here" %}

	Set and output page classes like Html.ClassForPage(); would do in Razor:
	{% ClassForPage, "custom-class" %}
	Or multiple classes:
	{% ClassForPage, "custom-class1", "custom-class2" %}
	Can be also used to simply output the page classes:
	{% ClassForPage %}

	Sets page classes like Html.AddPageClassNames(); in Razor:
	{% AddPageClassNames, "custom-class" %}
	Or multiple classes:
	{% AddPageClassNames, "custom-class1", "custom-class2" %}

#### Accessing the antiforgery token

	Displays a hidden form field with the antiforgery token. Equivalent to using Html.AntiForgeryTokenOrchard(); in Razor.
	{% AntiForgeryTokenOrchard %}
	Displays the value of the antiforgery token. Equivalent to using Html.AntiForgeryTokenValueOrchard(); in Razor.
	{% AntiForgeryTokenValueOrchard %}

#### Helpers

	Converts an URL to an app-relative one, similar to Href() in Razor.
	{% Href "~/Admin" %}
	If the site root URL is example.com then this will produce "/Admin", if there is an app path like example.com/Orchard then this will produce "/Orchard/Admin". These would do exactly the same:
	{% Href "/Admin" %}
	{% Href "Admin" %}

	So by utilizing the standard Liquid capture tag you can build dynamic URLs like following:
	{% capture profileUrl %}~/Profile/{{ Model.WorkContext.CurrentUser.UserName }}{% endcapture %}
	{% Href profileUrl %}
	Or even with multiple parameters:
	{% Href "~/Profile", Model.WorkContext.CurrentUser.UserName %}
	Or observe how we utilize capture, the Href tag and the RegisterLink to register a favicon with a dynamic URL:
	{% capture faviconUrl %}{% Href "~/Themes/MyTheme/Images/favicon.ico" %}{% endcapture %}
	{% RegisterLink, Href: faviconUrl, Rel: "shortcut icon", Title: "favicon", Type: "image/x-icon" %}


## Contribution notes

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-liquid-markup](https://bitbucket.org/Lombiq/orchard-liquid-markup) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Liquid-Markup](https://github.com/Lombiq/Orchard-Liquid-Markup) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.