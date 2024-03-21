using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public class HistoryTakingDb : DbContext{
    public HistoryTakingDb(DbContextOptions<HistoryTakingDb> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HistoryTakingDb).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Question> Questions {get; set;}
    public DbSet<Problem> Problems {get; set;}
    public DbSet<Examination> Examinations {get; set;}
    public DbSet<Treatment> Treatments {get; set;}
    public DbSet<QuestionExamination> QuestionExaminations {get; set;}
    public DbSet<QuestionProblem> QuestionProblems {get; set;}
    public DbSet<QuestionLog> QuestionLogs {get; set;}

    public DbSet<Diagnostic> Diagnostics {get; set;}
    public DbSet<Tag> Tags {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<StudentStats> StudentStats {get; set;}

    public DbSet<RefreshToken> RefreshTokens {get; set;}



}