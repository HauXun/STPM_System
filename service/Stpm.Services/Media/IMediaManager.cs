namespace Stpm.Services.Media;

public interface IMediaManager
{
  Task<string> SaveFileAsync(Stream buffer, string originalFileName, string contentType, MIMEType type = MIMEType.Picture, CancellationToken cancellationToken = default);

  Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}
