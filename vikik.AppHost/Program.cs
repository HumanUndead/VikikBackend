var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.KenChatAgent>("kenchatagent");

builder.Build().Run();
