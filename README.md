ConfigurationParser For .NET
==========

## Overview

[![Nuget](https://img.shields.io/nuget/v/DotNetCraft.ConfigurationParser.svg?style=flat)](https://www.nuget.org/packages/DotNetCraft.ConfigurationParser/) [![Build Status](https://travis-ci.org/DotNetCraft/ConfigurationParser.svg?branch=master)](https://travis-ci.org/DotNetCraft/ConfigurationParser) [![License LGPLv3](https://img.shields.io/badge/license-LGPLv3-green.svg)](http://www.gnu.org/licenses/lgpl-3.0.html)

Configuration Parser provides the most convenient way to parse configuration files of your applications. 
The parser is specifically designed to easily bind custom configuration sections. 
All required mapping and performing casts will be made by the Configuration Parser automatically.

## Install

ConfigurationParser can be found on nuget:

    PM> Install-Package DotNetCraft.ConfigurationParser

## Usage

Let’s imagine that our program requires settings for connecting to several external systems 
```C#
class ExternalSystemSettings
{
    public AuthenticationServiceSettings AuthenticationSettings { get; set; }
    public StaffServiceSettings StaffSettings { get; set; }
}

class AuthenticationServiceSettings
{
    public string Login { get; set; }
	public string Password { get; set; }
    public List<string> Urls { get; set; }
}

class StaffServiceSettings
{
	public Guid Token { get; set; }
	public string Url { get; set; }
}
```
In our configuration file we should do the following modifications
```xml
<!-- Wire up the handler inside the <configSections /> element; once per custom section -->
<section name="ExternalSystemSettings" type="DotNetCraft.ConfigurationParser.SimpleConfigurationSectionHandler, DotNetCraft.ConfigurationParser" />

<!-- Put this section in the main part of the config file -->
<ExternalSystemSettings>
	<AuthenticationSettings>
		<Login>DotNetCraft</Login>
		<Password>qwerty</Password>
		<Urls>
			<Url>https://github.com/DotNetCraft/ConfigurationParser</Url>
			<Url>https://github.com/DotNetCraft/ConfigurationParser</Url>
		</Urls>      
	</AuthenticationSettings>
	<StaffSettings Token="{D0C148F7-83C0-41B0-8F18-B47CAB09AD99}" Url="https://github.com/DotNetCraft/ConfigurationParser"/>
</ExternalSystemSettings>
```
Now we can easily read our configuration the following way:
```C#
ExternalSystemSettings externalSystemSettings = (ExternalSystemSettings)(dynamic)ConfigurationManager.GetSection("ExternalSystemSettings");
```
or
```C#
ExternalSystemSettings externalSystemSettings = (dynamic)ConfigurationManager.GetSection("ExternalSystemSettings");
```
## Using your own mapping strategy

If you wnat you can create your own mapping strategy for each fields.
```C#
class SmtpSettings
{
	public string Host { get; set; }
	public string Sender { get; set; }
	[CustomStrategy(typeof(SplitRecipientsCustomStrategy))]
	public List<string> Recipients { get; set; }
}
```
We added special attribute CustomStrategy and inserted typeof(SplitRecipientsCustomStrategy) as a parameter. SplitRecipientsCustomStrategy this is our custom strategy that you can find below.
```C#
class SplitRecipientsCustomStrategy : ICustomMappingStrategy
{
	public object Map(string input, Type itemType)
	{
		string[] items = input.Split(';');
		List<string> result = new List<string>();
		result.AddRange(items);
		return result;return input.Split(';').ToList();
	}

	public object Map(XmlNode xmlNode, Type itemType)
	{
		string input = xmlNode.InnerText;
		return Map(input, itemType);
	}
}
```
This is our configuration section
```xml
<SmtpSettings Host="gmail.com" Sender="no-reply">
	<Recipients>clien1@gmail.com;clien2@gmail.com;clien3@gmail.com</Recipients>
</SmtpSettings>
```
In the configuration above we inserted several emails splitted by semicomma. However, in the SmtpSettings we have List<string>. Thanks for SplitRecipientsCustomStrategy emails will be convertet from string into the list.

## Using mapping attribute

Sometimes you would like to store settings in the field that doesn't exist in the configuration file. However, you know what section and what attribute you would like to use. In this case you can use PropertyMappingAttribute
```C#
class TenantSettings
{
	[PropertyMapping("Name")]
	public string Tenant { get; set; }
}
```
In this case section will be like this
```xml
<TenantSettings Name="Bill"/>
```

## Conclusion

As you can see, ConfigurationParser makes the process of reading config setting simple and smooth and saves you a lot of time. Moreover, the utility is easily extensible thanks to ability to introduce new strategies. 
