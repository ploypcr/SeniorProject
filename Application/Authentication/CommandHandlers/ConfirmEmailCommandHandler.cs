using Application.Abstractions;
using Application.Authentication.Commands;
using MediatR;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
{
    private readonly IUserRepository _userRepository;
    public ConfirmEmailCommandHandler(IUserRepository userRepository){
        _userRepository = userRepository;
    }
    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id);
        if(user == null){
            throw new ArgumentNullException("Invalid user or token");
        }
        if(user.EmailVerificationToken != request.Token){
            throw new ArgumentNullException("Invalid user or token");
        }
        if(user.EmailVerified == true){
            throw new ArgumentNullException("Invalid user or token");
        }

        user.UpdateVerified();
        await _userRepository.UpdateUserAsync(user);

    }
}