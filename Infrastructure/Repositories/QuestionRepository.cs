using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository{
    private readonly HistoryTakingDb _context;

    public QuestionRepository(HistoryTakingDb context)
    {
        _context = context;
    }

    public async Task<Question> AddQuestion(Question q)
    {
        await _context.Questions.AddAsync(q);
        await _context.SaveChangesAsync();
        return q;
    }

    public async Task DeleteQuestionAsync(Question question)
    {
        var name = question.Name;
        _context.Questions.Remove(question);
        _context.Database.ExecuteSql($"UPDATE Questions SET Name=Name-1 WHERE Name > {name}");
        await _context.SaveChangesAsync();
    }
    public async Task<int> GetLastestName(){
        var questions = await _context.Questions.ToListAsync();
        if(questions.Count == 0){
            return 0;
        }
        return _context.Questions.Max(q =>q.Name);
    }
    public async Task<List<Question>> GetAllQuestionsAsync(List<string>? tagSearch)
    {
        IQueryable<Question> questionQuery = _context.Questions;
        if (tagSearch != null){
            questionQuery = questionQuery.Where(q => tagSearch.All(t1 => q.Tags.Any(t2 => t2.Name ==t1)));
        }
        return await questionQuery
            .Include(q => q.Problems)
            .Include(q => q.Examinations)
            .Include(q => q.Tags)
            .Include(q => q.Treatments)
            .Include(q => q.Diagnostics)
            .Include(q => q.Logs)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Question>> GetByDiagnosticAsync(DiagnosticId diagnosticId)
    {
        return await _context.Questions.Where(q => q.Diagnostics.Any(d => d.Id == diagnosticId)).ToListAsync();
    }

    public async Task<List<Question>> GetByExaminationAsync(ExaminationId examinationId)
    {
        return await _context.Questions.Where(q => q.Examinations.Any(qe => qe.ExaminationId == examinationId)).ToListAsync();
    }

    public async Task<Question?> GetByIdAsync(QuestionId questionId)
    {
        var result = await _context.Questions
            .Where(q => q.Id == questionId)
            .Include(q => q.Problems)
            .Include(q => q.Examinations)
            .Include(q => q.Tags)
            .Include(q => q.Treatments)
            .Include(q => q.Diagnostics)
            .Include(q => q.Logs)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        //Console.WriteLine(result.Examinations[1].Id);
        return result;
    }

    public async Task<Question?> GetByNameAsync(int name)
    {
        return await _context.Questions.FirstOrDefaultAsync(q => q.Name == name);
    }

    public async Task<List<Question>> GetByProblemAsync(ProblemId problemId)
    {
        return await _context.Questions.Where(q => q.Problems.Any(qp => qp.ProblemId == problemId)).ToListAsync();

    }

    public async Task<List<Question>> GetByTagAsync(TagId tagId)
    {
        return await _context.Questions.Where(q => q.Tags.Any(t => t.Id == tagId)).ToListAsync();

    }

    public async Task<List<Question>> GetByTreatmentAsync(TreatmentId treatmentId)
    {
        return await _context.Questions.Where(q => q.Treatments.Any(t => t.Id == treatmentId)).ToListAsync();
    }

    public async Task<QuestionExamination?> GetExaminationResult(QuestionId questionId, ExaminationId examinationId)
    {
        return await _context.QuestionExaminations.Where(q => q.QuestionId == questionId && q.ExaminationId == examinationId).FirstOrDefaultAsync();
    }

    public async Task UpdateQuestion(Question q)
    {
        _context.Questions.Update(q);
        await _context.SaveChangesAsync();
    }
}