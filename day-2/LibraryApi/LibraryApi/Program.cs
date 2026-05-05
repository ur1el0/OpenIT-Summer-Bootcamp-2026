var builder = WebApplication.CreateBuilder(args);  // setup config, logging, DI container
builder.Services.AddControllers();                 // register [ApiController] classes (Day 4: you add more services here)
builder.Services.AddEndpointsApiExplorer();        // make endpoints discoverable (for documentation tools)
var app = builder.Build();                         // finalize — after this line, no more services
app.MapControllers();                              // wire HTTP router → controller action methods
app.Run();                                         // start Kestrel server — blocks here until Ctrl+C
