using GymManagementSystem.Middleware;
using GymSystem.Data;
using GymSystem.Interfaces;
using GymSystem.Interfaces.IMember;
using GymSystem.Repository;
using GymSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Member
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();

// Membership Plan
builder.Services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
builder.Services.AddScoped<IMembershipPlanService, MembershipPlanService>();

// Member Membership
builder.Services.AddScoped<IMemberMembershipRepository, MemberMembershipRepository>();
builder.Services.AddScoped<IMemberMembershipService, MemberMembershipService>();
//payment 
builder.Services.AddScoped< IPaymentRepository,PaymentRepository>();
builder.Services.AddScoped< IPaymentService,PaymentService>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
