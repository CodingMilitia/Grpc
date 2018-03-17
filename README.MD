# Intro

A proof of concept for a way to implement gRPC services in a code first way using C# and .NET Core.

My initial idea was just create some helpers to host gRPC services in a similar way to ASP.NET Core, but then came across [this article](http://bartoszsypytkowski.com/c-using-grpc-with-custom-serializers/) by [Horusiath](https://github.com/Horusiath).

Also the initial idea to create something akin to IWebHost for a console application is also not required, as we have a [IHostedService](https://blogs.msdn.microsoft.com/cesardelatorre/2017/11/18/implementing-background-tasks-in-microservices-with-ihostedservice-and-the-backgroundservice-class-net-core-2-x/) in .NET Core 2.1.

# Some notes

The code only covers the most basic of scenarios, is not really well organized right now, need to review visibilities, naming, docs, etc. 

Right now I'm just getting a feel if something interesting and useful can be achieved.

If even in this form you can see something that could make all this shenanigans much better and useful, feel free to let me know.