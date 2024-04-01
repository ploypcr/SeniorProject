using Application.Abstractions;
using Application.Student.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.VisualBasic;

namespace Application.Student.CommandHandlers;

public class CreateStudentStatsCommandHandler : IRequestHandler<CreateStudentStatsCommand, StudentStats>
{
    private readonly IStatsRepository _statsRepository;
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly IExaminationRepository _examinationRepository;


    private readonly IDiagnosticRepository _diagnosticRepository;
    private readonly IQuestionRepository _questionRepository;

    public CreateStudentStatsCommandHandler(IExaminationRepository examinationRepository,IStatsRepository statsRepository, ITreatmentRepository treatmentRepository, IDiagnosticRepository diagnosticRepository, IQuestionRepository questionRepository, IProblemRepository problemRepository){
        _statsRepository = statsRepository;
        _treatmentRepository = treatmentRepository;
        _diagnosticRepository = diagnosticRepository;
        _questionRepository = questionRepository;
        _problemRepository = problemRepository;
        _examinationRepository = examinationRepository;
    }
    public async Task<StudentStats> Handle(CreateStudentStatsCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(new QuestionId(new Guid(request.QuestionId)));
        if(question == null){
            throw new ArgumentException("Don't have this question.");
        }

        var studentSelection = StudentStats.Create(
            request.UserId,
            new QuestionId(new Guid(request.QuestionId)));
        //Console.WriteLine(request.Problems);

        double problems1_score = 0;
        double wrong_problem1 = 0;
        double correct_problem1 = 0;

        double problems2_score = 0;
        double wrong_problem2 = 0;
        double correct_problem2 = 0;

        double examinations_score = 0;
        double wrong_exam = 0;
        double correct_exam = 0;

        double treatment_score = 0;
        double correct_treatment = 0;
        double wrong_treatment = 0;

        double diff_diagnostic_score = 0;
        double wrong_diff = 0;
        double correct_diff = 0;

        double ten_diagnostic_score = 0;
        double wrong_ten = 0;
        double correct_ten = 0;




        foreach(var p in request.Problems){
            if(await _problemRepository.GetByIdAsync(new ProblemId(new Guid(p.Id))) == null){
                throw new ArgumentException("No problem found.");
            }
            if(question.Problems.Any(qp => qp.ProblemId == new ProblemId(new Guid(p.Id)) && qp.Round == p.Round)){
                if(p.Round == 1){
                    correct_problem1++;
                }
                else{
                    correct_problem2++;
                }
            }else{
                if(p.Round == 1){
                    wrong_problem1++;
                }
                else{
                    wrong_problem2++;
                }
            }
            studentSelection.AddProblem(
                new ProblemId(new Guid(p.Id)),
                p.Round
            );
        }

        foreach(var e in request.Examinations){
            if(await _examinationRepository.GetByIdAsync(new ExaminationId(new Guid(e.Id))) == null){
                throw new ArgumentException("No examination found.");
            }
            if(question.Examinations.Any(qe => qe.ExaminationId == new ExaminationId(new Guid(e.Id)))){
                correct_exam++;
            }else{
                wrong_exam++;
            }
            studentSelection.AddExamination(
                new ExaminationId(new Guid(e.Id))
            );
        }
        foreach(var d in request.Diagnostics){
            var diagnostic = await _diagnosticRepository.GetByIdAsync(new DiagnosticId(new Guid(d.Id)));
            if(diagnostic == null){
                throw new ArgumentException("Diagnosis not found.");
            }
            if(question.Diagnostics.Any(qd => qd.Id == new DiagnosticId(new Guid(d.Id)))){
                Console.WriteLine(diagnostic.Type);
                if(diagnostic.Type == "tentative"){
                    correct_ten++;
                }
                if(diagnostic.Type == "differential"){
                    correct_diff++;
                }
                
            }else{
                if(diagnostic.Type == "tentative"){
                    wrong_ten++;
                }
                if(diagnostic.Type == "differential"){
                    wrong_diff++;
                }
            }
            studentSelection.AddDiagnostic(
                diagnostic
            );
        }
        foreach(var t in request.Treatments){
            var treatment = await _treatmentRepository.GetByIdAsync(new TreatmentId(new Guid(t.Id)));
            if(treatment == null){
                throw new ArgumentException("Treatment not found.");
            }
            if(question.Treatments.Any(qt => qt.Id == new TreatmentId(new Guid(t.Id)))){
                correct_treatment++;
                
            }else{
                wrong_treatment++;
            }
            studentSelection.AddTreatment(
                treatment
            );
        }
        Console.WriteLine(request.HeartProblem1);
        Console.WriteLine(correct_problem1);
        Console.WriteLine(wrong_problem1);

        problems1_score += (12.5/question.Problems.Where(p => p.Round == 1).Count())*correct_problem1*(((double)request.HeartProblem1)/5);
        problems1_score -= (12.5/question.Problems.Where(p => p.Round == 1).Count())*0.5*wrong_problem1;
        problems1_score  = problems1_score > (double)0 ? problems1_score : 0;

        problems2_score += (12.5/question.Problems.Where(p => p.Round == 2).Count())*correct_problem2*(((double)request.HeartProblem2)/5);
        problems2_score -= (12.5/question.Problems.Where(p => p.Round == 2).Count())*0.5*wrong_problem2;
        problems2_score  = problems2_score > (double)0 ? problems2_score : 0;

        examinations_score += (25/question.Examinations.Count())*correct_exam;
        examinations_score -= (25/question.Examinations.Count())*0.5*wrong_exam;
        examinations_score  = examinations_score > (double)0 ? examinations_score : 0;

        treatment_score += (25/question.Treatments.Count())*correct_treatment;
        treatment_score -= (25/question.Treatments.Count())*0.5*wrong_treatment;
        treatment_score  = treatment_score > (double)0 ? treatment_score : 0;

        diff_diagnostic_score += (12.5/question.Diagnostics.Where(d => d.Type == "differential").Count())*correct_diff;
        diff_diagnostic_score -= 12.5*question.Diagnostics.Where(d => d.Type == "differential").Count()*0.5*wrong_diff;
        diff_diagnostic_score  = diff_diagnostic_score > (double)0.00 ? diff_diagnostic_score : 0;

        ten_diagnostic_score += (12.5/question.Diagnostics.Where(d => d.Type == "tentative").Count())*correct_ten;
        ten_diagnostic_score -= 12.5*question.Diagnostics.Where(d => d.Type == "tentative").Count()*0.5*wrong_ten;
        ten_diagnostic_score  = ten_diagnostic_score > (double)0.00 ? ten_diagnostic_score : 0;

        studentSelection.SetScore(
            problems1_score,
            problems2_score,
            examinations_score,
            treatment_score,
            diff_diagnostic_score,//treatment_score,
            ten_diagnostic_score);//diagnostic_score);
            
        await _statsRepository.AddStudentStats(studentSelection);
        return studentSelection;
    }
}