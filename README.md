Restaurant Order System – Orleans-Based 

A distributed Restaurant Order System built using Microsoft Orleans, leveraging the Virtual Actor Model for scalable and fault-tolerant order processing.

 1. Project Overview

This system manages restaurant orders using Orleans Grains, persisting state and handling clustering via PostgreSQL. It supports API interaction via Swagger and provides monitoring through the Orleans Dashboard.

 2. Technologies Used

.NET 8.0
Microsoft Orleans 9.1.2
PostgreSQL 16.8 (via ADO.NET for clustering and persistence)
Swagger (API testing)
Orleans Dashboard (real-time monitoring)
xUnit + Moq (unit testing)

 3. Database Setup

 Create the database:
CREATE DATABASE orleans_orders;

4. Orleans System Tables (PostgreSQL)

OrleansMembershipTable: Used by Orleans Silo Clustering. Tracks the status (active/inactive) of silos in the cluster.


OrleansMembershipVersionTable: Used by Silo Clustering. Handles versioning to ensure consistency when updating silo membership.


OrleansStorage: Used for Grain Persistence. Stores the state of grains that implement IGrainState.


OrleansRemindersTable: Used by the Reminders System. Stores and manages timers/reminders across silos.


OrleansQuery: Used for the Query Registry. Stores reusable SQL queries, such as cleanup or maintenance operations.


 Download scripts from Orleans GitHub.(https://github.com/dotnet/orleans)

 5. Required Cleanup Query

Register reusable cleanup logic for defunct silos:

INSERT INTO OrleansQuery(QueryKey, QueryText) VALUES ( 'CleanupDefunctSiloEntriesKey', ' DELETE FROM OrleansMembershipTable WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL AND (Status = 4 OR Status = 5); -- Dead or ShuttingDown ' );

6. Functional Grains
OrderGrain: Responsible for managing the complete order lifecycle. This includes creating new orders, updating existing ones, and handling order cancellations.

 7. Required NuGet Packages
Install the following packages:
dotnet add package Microsoft.Orleans.Server
dotnet add package Microsoft.Orleans.Persistence.AdoNet
dotnet add package Microsoft.Orleans.Hosting
dotnet add package Swashbuckle.AspNetCore
dotnet add package Moq
dotnet add package xunit

 8. Orleans Concepts

Orleans
Distributed app framework built on the Virtual Actor Model by Microsoft.

Virtual Actor Model
Orleans automatically manages the lifecycle and state of grains. Developers work with grains like regular objects while Orleans handles:
Activation/deactivation


Persistence


Concurrency


Grains & Factory Usage
var orderGrain = grainFactory.GetGrain<IOrderGrain>(orderId);
await orderGrain.PlaceOrder(orderDto);


 9. Configuration Sample (appsettings.json)

 "ConnectionStrings": { "OrleansDb": "Host=localhost;Database=orleans_orders;Username=postgres;Password=password" }, "AdoNet": { "Invariant": "Npgsql", "ConnectionString": "Host=localhost;Database=orleans_orders;Username=postgres;Password=password" }, "ClusterOptions": { "ClusterId": "restaurant-cluster", "ServiceId": "RestaurantOrderSystem" }

10. Configuration Explanation

ConnectionStrings.OrleansDb: Used by Orleans Runtime. Specifies the connection string to PostgreSQL, where Orleans system tables are stored.


ClusterOptions.ClusterId: Used by Orleans. This uniquely identifies the Orleans cluster (e.g., "restaurant-cluster").


ClusterOptions.ServiceId: Used by Orleans. This identifies the distributed application domain (e.g., "RestaurantOrderSystem").


Logging.LogLevel.: Used by .NET Logging. This controls how verbose the logging output is during development or production.


11. Testing Strategy
 Verified:
Order placement + cancellation logic
State persistence
API endpoints via Swagger
Unit testing with mocks

Unit Test Example (Moq)

var mockOrderGrain = new Mock(); mockOrderGrain.Setup(o => o.PlaceOrder(It.IsAny())) .Returns(Task.CompletedTask);

12. API Testing (Swagger)

Enable Swagger in Program.cs:

builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen();

Swagger UI: http://localhost:7099/swagger

13. Orleans Dashboard

Enable Orleans dashboard monitoring:

siloBuilder.UseDashboard(options => options.HostSelf = true);

Dashboard UI: http://localhost:8080

 14. References

 Orleans Official Docs(https://learn.microsoft.com/en-us/dotnet/orleans/)
 ADO.NET Setup Scripts(https://github.com/dotnet/orleans/tree/main/src/AdoNet)
Intro to Orleans – YouTube(https://www.youtube.com/results?search_query=microsoft+orleans)

