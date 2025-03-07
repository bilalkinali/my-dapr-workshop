using Aspire.Hosting.Dapr;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var statestore = builder.AddDaprStateStore("pizzastatestore");
var pubsubComponent = builder.AddDaprPubSub("pizzapubsub");

builder.AddProject<PizzaOrder>("pizzaorderservice")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "pizza-order",
        DaprHttpPort = 3501
    })
    .WithReference(statestore)
    .WithReference(pubsubComponent);

builder.AddProject<PizzaKitchen>("pizzakitchenservice")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "pizza-kitchen",
        DaprHttpPort = 3503
    })
    .WithReference(pubsubComponent);

builder.AddProject<PizzaStorefront>("pizzastorefrontservice")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "pizza-storefront",
        DaprHttpPort = 3502
    })
    .WithReference(pubsubComponent);

builder.AddProject<PizzaDelivery>("pizzadeliveryservice")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "pizza-delivery",
        DaprHttpPort = 3504
    })
    .WithReference(pubsubComponent);

//builder.AddProject<Projects.PizzaOrder>("pizzaorder");

//builder.AddProject<Projects.PizzaStorefront>("pizzastorefront");

//builder.AddProject<Projects.PizzaKitchen>("pizzakitchen");

//builder.AddProject<Projects.PizzaDelivery>("pizzadelivery");

builder.Build().Run();
