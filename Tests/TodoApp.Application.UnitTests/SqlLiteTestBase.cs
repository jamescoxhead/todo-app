using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Application.IntegrationTests;

public abstract class SqlLiteTestBase : IDisposable
{
    private readonly DbContextOptions<TodoDbContext> dbContextOptions;
    private readonly DbConnection dbConnection;
    private bool isDisposed;

    public SqlLiteTestBase()
    {
        this.dbConnection = new SqliteConnection("Filename=:memory:");
        this.dbConnection.Open();

        this.dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite(this.dbConnection)
            .Options;

        using var dbContext = this.CreateDbContext;
        dbContext.Database.EnsureCreated();
    }

    protected TodoDbContext CreateDbContext => new(this.dbContextOptions);

    [OneTimeTearDown]
    public void TestFixtureTearDown() => this.Dispose();

    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!this.isDisposed)
        {
            if (disposing)
            {
                this.dbConnection.Dispose();
            }

            this.isDisposed = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
