# Dime.WebSockets

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/System%20-%20MASTER%20-%20CI?branchName=master) [![Dime.WebSockets package in Dime.Scheduler feed in Azure Artifacts](https://feeds.dev.azure.com/dimenicsbe/_apis/public/Packaging/Feeds/a7b896fd-9cd8-4291-afe1-f223483d87f0/Packages/403c6937-3547-4722-a387-81242aa5a2aa/Badge)](https://dev.azure.com/dimenicsbe/Dime.Scheduler%20V2/_packaging?_a=package&feed=a7b896fd-9cd8-4291-afe1-f223483d87f0&package=403c6937-3547-4722-a387-81242aa5a2aa&preferRelease=true)

## Introduction

This is a simple library that can be used to keep track of SignalR connections. This is useful in multi-tenant instances where you don't want to broadcast data to other tenants.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

This project provides a simple interface and a few implementations for that interface. This makes it configurable and easy to setup in the startup of the application. For example, it requires only 1 line of code to swap an in-memory connection tracker with a SQL connection tracker.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Kendo:

`dotnet add package Dime.WebSockets`

## Usage

When a user connects, the connection can be stored in the connection tracker's storage medium. This data can be retrieved when the hub broadcasts data to the clients.

```csharp
public class MyHub : Hub
{
    private readonly IConnectionTracker<Connection> _connectionTracker;

    public MyHub(IConnectionTracker<Connection> connectionTracker)
    {
        _connectionTracker = connectionTracker;
    }

    public override async Task OnConnected()
    {
      await this.ConnectionTracker.AddAsync(new Connection(){ ConnectionId = Context.ConnectionId });
    }
}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)