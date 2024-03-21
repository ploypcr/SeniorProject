using Application.Abstractions;
using Application.Questions.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Questions.CommandHandlers;

public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand>{
    private readonly IQuestionRepository _questionRepository;
    public DeleteQuestionHandler(IQuestionRepository questionRepository){
        _questionRepository = questionRepository;
    }
    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question == null){
            throw new Exception("Question not found");
        }

        await _questionRepository.DeleteQuestionAsync(question);
    }
}