using EatTogether.Models.EfModels;
using EatTogether.Models.Extensions;
using EatTogether.Models.Infra;
using EatTogether.Models.Repositories;
using EatTogether.Models.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EatTogether
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			// ���U��DBContext
			builder.Services.AddDbContext<EatTogetherDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// �s�W JWT Authentication
			var jwtSettings = builder.Configuration.GetSection("Jwt");
			var secretKey = jwtSettings["SecretKey"]!;

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				// �q httpOnly Cookie Ū�� Token
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = ctx =>
					{
						ctx.Token = ctx.Request.Cookies["jwt"];
						return Task.CompletedTask;
					}
				};

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidAudience = jwtSettings["Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
												  Encoding.UTF8.GetBytes(secretKey)),
					ClockSkew = TimeSpan.Zero
				};
			});

			// ���URepository
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IDishRepository, DishRepository>();
			builder.Services.AddScoped<ISetMealRepository, SetMealRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // ���UService
            builder.Services.AddScoped<CategoryService>();
			builder.Services.AddScoped<DishService>();
			builder.Services.AddScoped<SetMealService>();
            builder.Services.AddScoped<ProductService>();


            builder.Services.AddScoped<ITableRepository, TableRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<ICouponRepository, CouponRepository>();
            builder.Services.AddScoped<IMemberCouponRepository, MemberCouponRepository>();
            builder.Services.AddScoped<TableService>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<CouponService>();
            builder.Services.AddScoped<ReservationEmailService>();
            builder.Services.AddScoped<BirthdayCouponService>();
            builder.Services.AddHostedService<BirthdayCouponBackgroundService>();
            builder.Services.AddHostedService<CouponNotifyBackgroundService>();

			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
			builder.Services.AddScoped<IRoleRepository, RoleRepository>();
			builder.Services.AddScoped<IFunctionRepository, FunctionRepository>();
			builder.Services.AddScoped<IRoleFunctionRepository, RoleFunctionRepository>();
			builder.Services.AddScoped<IMemberRepository, MemberRepository>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IRoleService, RoleService>();
			builder.Services.AddScoped<IMemberService, MemberService>();
			builder.Services.AddScoped<IPasswordResetEmailService, PasswordResetEmailService>();

            // 結帳相關
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<EcPayService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPreOrderRepository, PreOrderRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IReportService, ReportService>();



			builder.Services.AddScoped<IEventRepository, EventRepository>();
			builder.Services.AddScoped<EventService>();
			builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
			builder.Services.AddScoped<ArticleCategoryService>();
			builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
			builder.Services.AddScoped<ArticleService>();



			// ���U Infra�]�ݭn DI ���~���U�^
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<JwtHelper>();
			builder.Services.AddSingleton<UserNumberGenerator>();

			var app = builder.Build();

            //�C������A���t�Φ۰ʶ]���ʪ����A
			using (var scope = app.Services.CreateScope())
			{
				EventInitializerExtensions.UpdateEventStatuses(app.Services);
			}



			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
            }

			// ������~������
			app.UseStatusCodePagesWithReExecute("/Error/{0}");

			app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			// �s�W Authentication �b Authorization ���e
			app.UseAuthentication();

			app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
