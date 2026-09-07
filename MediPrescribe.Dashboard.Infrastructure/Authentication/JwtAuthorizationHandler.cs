using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MediPrescribe.Dashboard.Infrastructure.Authentication;

/// <summary>
/// يضيف رأس Authorization: Bearer {token} تلقائيًا لكل طلب يصدر إلى الـ API.
/// يتم جلب الـ Token من Claim داخل ملف التعريف (Cookie) للمستخدم المسجّل حاليًا.
/// </summary>
public sealed class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.User.FindFirstValue("access_token");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}