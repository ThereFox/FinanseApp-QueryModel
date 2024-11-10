using Application;
using Application.Events.Realisation;
using Infrastructure.StateUpdator;
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
    .RegistrateStateUpdateEventHandler<BillAmountChangeEvent, BillAmountChangeHandler>("amountChanges")
    .RegistrateStateUpdateEventHandler<BillCreatedEvent, CreateBillHandler>("createBill");

builder.Services
    .AddApplication()
    .AddPersistense(connectionString);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
