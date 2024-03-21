using Application.Questions.Commands;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services;

public interface IFileStorageService{
        Task<string> UploadExaminationImg( string examinationId, IFormFile? file, byte[]? imageBytes ,string? oldImg);

    void DeleteFile(string filePath);
    void DeleteDirectory(string examinationId);


    bool CheckIfFilePathExist(string filePath);

    Task<List<CreateQuestionCommand>> ImportExcelFileToDB(IFormFile file, string userId);
    
    Task<byte[]> GetExcelTemplate();
}