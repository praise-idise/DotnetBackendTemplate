namespace BackendTemplate.Application.Services.Auth.DTOs;

public record ResetPasswordDTO(string Email, string Token, string NewPassword);

