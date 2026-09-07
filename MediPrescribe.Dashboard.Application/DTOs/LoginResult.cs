namespace MediPrescribe.Dashboard.Application.DTOs;

public enum LoginResultStatus
{
    Success,
    InvalidCredentials,
    InvalidRequest,
    ServerError
}

public class LoginResult
{
    public LoginResultStatus Status { get; set; }

    public LoginResponseDto? Data { get; set; }

    public static LoginResult Ok(LoginResponseDto data) =>
        new() { Status = LoginResultStatus.Success, Data = data };

    public static LoginResult InvalidCredentials() =>
        new() { Status = LoginResultStatus.InvalidCredentials };

    public static LoginResult InvalidRequest() =>
        new() { Status = LoginResultStatus.InvalidRequest };

    public static LoginResult ServerError() =>
        new() { Status = LoginResultStatus.ServerError };
}
