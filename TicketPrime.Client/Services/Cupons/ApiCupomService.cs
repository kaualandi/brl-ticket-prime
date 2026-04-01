using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace TicketPrime.Client.Services.Cupons;

public sealed class ApiCupomService(HttpClient http) : ICupomService
{
    public async Task<CriarCupomResult> CriarAsync(CriarCupomRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PostAsJsonAsync("/api/cupons", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            return CriarCupomResult.Ok();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return CriarCupomResult.Fail(await ExtractErrorMessageAsync(response, cancellationToken));
        }

        return CriarCupomResult.Fail("Nao foi possivel cadastrar o cupom agora. Tente novamente em instantes.");
    }

    public async Task<IReadOnlyList<CupomListItem>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync("/api/cupons", cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var data = await response.Content.ReadFromJsonAsync<List<CupomListItem>>(cancellationToken) ?? [];
            return data;
        }

        return [];
    }

    public async Task<CupomListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync($"/api/cupons/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<CupomListItem>(cancellationToken: cancellationToken);
        }

        return null;
    }

    public async Task<CriarCupomResult> AtualizarAsync(int id, AtualizarCupomRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PutAsJsonAsync($"/api/cupons/{id}", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return CriarCupomResult.Ok();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return CriarCupomResult.Fail(await ExtractErrorMessageAsync(response, cancellationToken));
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return CriarCupomResult.Fail("Cupom nao encontrado para edicao.");
        }

        return CriarCupomResult.Fail("Nao foi possivel atualizar o cupom agora. Tente novamente em instantes.");
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(rawContent))
        {
            return "A API retornou um erro de validacao sem detalhes.";
        }

        try
        {
            var problem = JsonSerializer.Deserialize<ValidationProblemDetailsResponse>(rawContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (problem is not null)
            {
                if (!string.IsNullOrWhiteSpace(problem.Detail))
                {
                    return problem.Detail;
                }

                if (!string.IsNullOrWhiteSpace(problem.Title))
                {
                    return problem.Title;
                }

                if (problem.Errors is not null)
                {
                    var firstError = problem.Errors
                        .SelectMany(entry => entry.Value ?? Array.Empty<string>())
                        .FirstOrDefault(message => !string.IsNullOrWhiteSpace(message));

                    if (!string.IsNullOrWhiteSpace(firstError))
                    {
                        return firstError;
                    }
                }
            }
        }
        catch
        {
        }

        return rawContent.Trim('"');
    }

    private sealed class ValidationProblemDetailsResponse
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
