Utilities.NET
=============

Utility Classes and Snippet for C# .NET that i pick up or develop over time

## CryptoConfigurationManager

### Get AppSettings example

Example **App.config**:

	<appSettings>
    	<add key="Password" value="unprotected" />
    </appSettings>

Example code:

    var password = CryptoConfigurationManager.AppSettings["Password"];
    Console.WriteLine(password); // stdOut: unprotected
    
Now the example **App.config** will look like this

	<appSettings>
    	<add key="Password" value="V7eyFMDnU+NGj6PoLymk5WG+XPG33mot+3J2EaL7x6o=" />
    </appSettings>
    
The `CryptoConfigurationManager` will try to decrypt the string, and when it fails realize that the string is not encrypted and encrypt it before it returns the decrypted result.

### Set AppSettings example

Example **App.config**:

	<appSettings>
    	<add key="Password" value="" />
    </appSettings>
    
Example code:

	CryptoConfigurationManager.AppSettings["Password"] = "unprotected";
    
Now the example **App.config** will look like this

	<appSettings>
    	<add key="Password" value="V7eyFMDnU+NGj6PoLymk5WG+XPG33mot+3J2EaL7x6o=" />
    </appSettings>
    
The `CryptoConfigurationManager` has now encrypted the string and put the value with the correct key in the appSettings section.

The MIT License (MIT)
=============

Copyright (c) 2013 Sander Struijk

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
