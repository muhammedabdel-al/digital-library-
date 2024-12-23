    using Microsoft.EntityFrameworkCore;
    using project.Database;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var conStr = builder.Configuration.GetConnectionString("SqlCon");
    // Add services to the container.
    //builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<AppDBContext>(options =>
    {
        options.UseSqlServer(conStr,
    b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
    }, ServiceLifetime.Scoped);

    builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(1); });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();
    app.UseSession();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=PersonController1}/{action=Login}/{id?}");

    app.Run();
