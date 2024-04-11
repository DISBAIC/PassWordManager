A simple PassWordManager ,writing by Csharp, made with WPF UI.
The reason why I develop it is some people are constantly trying to log in my Microsoft account,
even successfully logged into my account.
The Authenticator has remind me and I have prevented that.
I have realized that the security of my password is poor;
So,I develop this appliaction to deal with that.

This appliaction won't save any password.It is a password generating tool.
It works with some informations,Just like the website name,or a number you will never forget.

The password are not part of the log file ,each encryption record was writed with JSON format.
The full log with contain the checksum(checkint) and other information.
The simple log only contained the critical information to help you to generate it again.

The log file path : ../Logs/PDELog.Json
The defaut original password is "password";
You can modify the password generating algorithm . 
Left click the one of the five buttons will enable it.Right click will adjusting the order.
The silder is used to increase security in scenarios where password length is not restricted.
Each level will increase by four digits in length.

My next plan is to make this appliaction to a browser extension.

[WPF UI](https://wpfui.lepo.co/)
