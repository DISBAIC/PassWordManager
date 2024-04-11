A simple PassWordManager made with WPF UI ,writing by Csharp.
The reason why I develop it is some people are constantly trying to log in my Microsoft account,
even them have log in my account successfully ,my verification email was almost changed.
The Authenticator has remind me and I have prevented that.
So,I develop this appliaction to deal with that.

The password are not part of the log file ,each encrypted record was writed with JSON format.
The full log with contain the checksum(checkint) and other information.
The simple log only contained the cricuit information to help you to generate it again.

The log file path : ../Logs/PDELog.Json
The defaut original password is "password";
You can modify the password generating algorithm . 
Left click the one of the five buttons will enable it.Right click will adjusting the order.
The silder is used to increase security in scenarios where password length is not restricted.
Each level will increase by four digits in length.

My next plan is to make this appliaction to a browser extension.
