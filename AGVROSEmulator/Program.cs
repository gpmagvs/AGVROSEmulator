using AGVROSEmulator;
using AGVROSEmulator.IOModule;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();




Utility.AGVEmulator.Initialize();

Console.WriteLine($"AGV¨®´Ú={ConfigurationHelper.Instance.AgvType}");

switch (ConfigurationHelper.Instance.AgvType)
{
    case AGV_TYPE.FORK:
        Utility.IOModule = new ForkAGVIOModule();
        break;
    case AGV_TYPE.SUBMARINE:
        Utility.IOModule = new SubmarineAGVIOModule();
        break;
    case AGV_TYPE.INSPECTION:
        break;
    case AGV_TYPE.DEMO_INSPECTION:
        Utility.IOModule = new DemoRoomInspectionAGVIOModule();
        break;
    default:
        break;
}
_ = Utility.IOModule.Connect("0.0.0.0", Port: ConfigurationHelper.Instance.WagoPort);

Console.Title = "¨®±±¼ÒÀÀ¾¹V1";

app.Run();
