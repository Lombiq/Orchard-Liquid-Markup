# Orchard Liquid Markup Readme



## Project Description

Orchard module for adding support for templates written in Liquid Markup (http://liquidmarkup.org/). Uses DotLiquid (http://dotliquidmarkup.org/).


## Documentation

### Overview

This module adds the ability to use shape templates written in [Liquid Markup](http://liquidmarkup.org/) and uses the [DotLiquid library](http://dotliquidmarkup.org/). [See what Liquid offers](https://github.com/Shopify/liquid/wiki/Liquid-for-Designers). The module is also available for [DotNest](http://dotnest.com/) sites for templating.

Naming conventions are C#-style: this means that all the properties you can access on viewmodels from your templates will have the same names as usual but built-in Liquid filters (like upcase) will also behave in the same way (i.e. you'll be able to use it as Upcase).

There are the following features in the module:

- Liquid Markup: doesn't provide any user-accessible features.
- Liquid Markup Templates: extends Orchard.Templates so you can write Liquid templates from the admin.
- Liquid Markup View Engine: adds a view engine that enables you to use .liquid Liquid-formatted templates in your themes and modules. You can use this to develop Liquid templates quickly from an IDE.

Note that since Liquid was designed to be safe you can't call arbitrary methods on services that may be accessible from templates.

### Examples

#### Accessing the model

	Accessing shape properties:
	{{ Model.Id }}
	{{ Model.Metadata.Type }}
	
	Accessing the viewmodel from the admin User shape:
	{{ Model.CurrentUser.UserName }}
	{{ Model.CurrentUser.Email }}
	
	Dynamic expressions on ContentItems work, i.e. you can do this from the Content shape or content part shapes:
	{{ Model.ContentItem.TitlePart.Title }}

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
	{% style '/url/to/stylesheet.css' %}
	You can omit the single quotes or use double quotes instead if you wish.
	
	{% script '/url/to/script.js', head %}
	The second parameter is the script location: head or foot. The default is foot so you can omit the parameter if you want to include the script in the footer.
	
	You can also reference resources by their names if they are defined in an enabled feature:
	{% scriptrequire 'jQueryUI', head %}
	{% stylerequire 'jQueryUI_Orchard' %}

#### Working with shapes

	Displaying shapes from the viewmodel with the Display filter, e.g. from the Content shape:
	<article>
	{{ Model.Content | Display }}
	</article>
	Note that there are no quotes around Model.Content!
	
	Displaying any shape with the display tag, here the User shape:
	{% display User %}
	For the sake of consistency the display tag (although tags are all lowercase in Liquid) is also available with a capital D.
	
	You can also give the shape input parameters as from C#:
	{% display User, 'Parameter1: some value', 'Parameter2: some other value' %}
	These then can be use from inside the User shape as Model.ParameterName.

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-liquid-markup](https://bitbucket.org/Lombiq/orchard-liquid-markup) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Liquid-Markup](https://github.com/Lombiq/Orchard-Liquid-Markup) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.