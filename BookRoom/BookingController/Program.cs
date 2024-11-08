using BOs.Entity;
using BOs.Filer;
using DAOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using REPOs;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using System.Text.Json.Serialization;
using Net.payOS;
using BookingController.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Nạp appsettings.json trước khi cấu hình JWT
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Add services to the container.
/*builder.Services.AddControllers();*/
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Giữ nguyên tên thuộc tính
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:7046", "https://bookingcontroller.azurewebsites.net", "https://pod-booking-ui56.vercel.app") // Thêm địa chỉ frontend hoặc các địa chỉ khác nếu cần
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Cho phép gửi cookie với xác thực Google
    });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Cấu hình để thêm nút Authorize
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Vui lòng nhập token JWT với định dạng 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//Add
builder.Services.AddScoped<IGuestDAO, GuestDAO>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IAccountDAO, AccountDAO>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBranchDAO, BranchDAO>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IRoomDAO, RoomDAO>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ISlotDAO, SlotDAO>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();
builder.Services.AddScoped<IRoomTypeDAO, RoomTypeDAO>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IBookingDAO, BookingDAO>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IStatusDAO, StatusDAO>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IPaymentDAO, PaymentDAO>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISlotBookingDAO, SlotBookingDAO>();
builder.Services.AddScoped<ISlotBookingRepository, SlotBookingRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();





// Cấu hình dịch vụ CORS



// Nạp JWT Secret Key từ cấu hình
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new Exception("JWT Secret Key is missing or empty in appsettings.json");
}

// Đảm bảo rằng IJWTService được khởi tạo đúng cách
builder.Services.AddScoped<IJWTService>(provider =>
    new JWTService(jwtSecretKey));

// Cấu hình Authentication và JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;


})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Để dễ dàng cho phát triển
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)), // Sử dụng jwtSecretKey
        ValidateIssuer = false,
        ValidateAudience = false
    };
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "18095045907-g5tnnh8fqge9oskrdorr5nn5fj5en12p.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/signin-google"; // Đường dẫn callback đã cấu hình trong Google Console
});
// Nạp cấu hình từ appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Khởi tạo PayOS với các thông tin cấu hình từ appsettings.json
PayOS payOS = new PayOS(
    configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Không tìm thấy PAYOS_CLIENT_ID"),
    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Không tìm thấy PAYOS_API_KEY"),
    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Không tìm thấy PAYOS_CHECKSUM_KEY")
);

// Đăng ký PayOS như một dịch vụ Singleton
builder.Services.AddSingleton(payOS);

// Đảm bảo cấu hình DbContext
builder.Services.AddDbContext<BookingRoommContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<BookingRoommContext>();

var app = builder.Build();

// Sử dụng CORS
app.UseCors("AllowSpecificOrigin");

// Thực hiện migrate database và seed roles
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookingRoommContext>();
    context.Database.Migrate();

    // Seed default roles
    if (!context.Roles.Any())
    {
        context.Roles.AddRange(
            new Role { RoleName = "Admin" },
            new Role { RoleName = "Staff" },
            new Role { RoleName = "Customer" }
        );
        context.SaveChanges();
    }
}

/*// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.MapGet("/login-google", async (HttpContext httpContext) =>
{
    // Thực hiện Google Authentication
    await httpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/google-login-callback" // Sau khi đăng nhập thành công, chuyển đến endpoint này
    });
});
// Endpoint xử lý sau khi đăng nhập thành công
app.MapGet("/google-login-callback", async (HttpContext httpContext) =>
{
    if (httpContext.User.Identity?.IsAuthenticated == true)
    {
        var claims = httpContext.User.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        return Results.Ok(new { Email = email, Name = name });
    }

    return Results.Unauthorized();
});

app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingController v1");
    options.RoutePrefix = "swagger";
});




app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
