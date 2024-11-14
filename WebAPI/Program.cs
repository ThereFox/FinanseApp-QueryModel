using Application;
using Application.Events.Realisation;
using Infrastructure.StateUpdator;
using Infrastructure.StateUpdator.EventReader.Service;
using Persistense;
using Persistense.Dapper.StateUpdator.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetValue<string>("ConnectionString");

builder
    .Services
    .AddStateUpdating()
    .RegistrateStateUpdateEventHandler<BillAmountChangeEvent, BillAmountChangeHandler>("BillAmountChange")
    .RegistrateStateUpdateEventHandler<BillCreatedEvent, CreateBillHandler>("BillCreated")
    .RegistrateStateUpdateEventHandler<ClientCreatedEvent, CreateClientHandler>("ClientCreated")
    .AddBackgroundDBUpdator();

builder.Services
    .AddApplication()
    .AddPersistense(connectionString);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
