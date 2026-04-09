using System.Net;
using System.Net.Http.Json;

namespace TicketPrime.Client.Services.Eventos;

public sealed class ApiEventoService(HttpClient http) : IEventoService
{
    public async Task<IReadOnlyList<EventoListItem>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync("/api/eventos", cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var data = await response.Content.ReadFromJsonAsync<List<EventoListItem>>(cancellationToken) ?? [];
            return data;
        }

        return [];
    }

    public async Task<EventoListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync($"/api/eventos/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (response.StatusCode == HttpStatusCode.OK)
            return await response.Content.ReadFromJsonAsync<EventoListItem>(cancellationToken: cancellationToken);

        return null;
    }

    public async Task<EventoResult> CriarAsync(SalvarEventoRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PostAsJsonAsync("/api/eventos", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Created)
            return EventoResult.Ok();

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return EventoResult.Fail(await response.Content.ReadAsStringAsync(cancellationToken));

        return EventoResult.Fail("Nao foi possivel criar o evento agora. Tente novamente em instantes.");
    }

    public async Task<EventoResult> AtualizarAsync(int id, SalvarEventoRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PutAsJsonAsync($"/api/eventos/{id}", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
            return EventoResult.Ok();

        if (response.StatusCode == HttpStatusCode.NotFound)
            return EventoResult.Fail("Evento nao encontrado.");

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return EventoResult.Fail(await response.Content.ReadAsStringAsync(cancellationToken));

        return EventoResult.Fail("Nao foi possivel atualizar o evento agora. Tente novamente em instantes.");
    }

    public async Task<EventoResult> DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await http.DeleteAsync($"/api/eventos/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent)
            return EventoResult.Ok();

        if (response.StatusCode == HttpStatusCode.NotFound)
            return EventoResult.Fail("Evento nao encontrado.");

        return EventoResult.Fail("Nao foi possivel excluir o evento agora. Tente novamente em instantes.");
    }
}
