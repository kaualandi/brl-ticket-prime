using System.Net;
using System.Net.Http.Json;

namespace TicketPrime.Client.Services.Usuarios;

public sealed class ApiUsuarioService(HttpClient http) : IUsuarioService
{
    public async Task<IReadOnlyList<UsuarioListItem>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync("/api/usuarios", cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var data = await response.Content.ReadFromJsonAsync<List<UsuarioListItem>>(cancellationToken) ?? [];
            return data;
        }

        return [];
    }

    public async Task<UsuarioListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync($"/api/usuarios/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (response.StatusCode == HttpStatusCode.OK)
            return await response.Content.ReadFromJsonAsync<UsuarioListItem>(cancellationToken: cancellationToken);

        return null;
    }

    public async Task<UsuarioResult> CriarAsync(SalvarUsuarioRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PostAsJsonAsync("/api/usuarios", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Created)
            return UsuarioResult.Ok();

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return UsuarioResult.Fail(await response.Content.ReadAsStringAsync(cancellationToken));

        return UsuarioResult.Fail("Nao foi possivel cadastrar o usuario agora. Tente novamente em instantes.");
    }

    public async Task<UsuarioResult> AtualizarAsync(int id, SalvarUsuarioRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PutAsJsonAsync($"/api/usuarios/{id}", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
            return UsuarioResult.Ok();

        if (response.StatusCode == HttpStatusCode.NotFound)
            return UsuarioResult.Fail("Usuario nao encontrado.");

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return UsuarioResult.Fail(await response.Content.ReadAsStringAsync(cancellationToken));

        return UsuarioResult.Fail("Nao foi possivel atualizar o usuario agora. Tente novamente em instantes.");
    }

    public async Task<UsuarioResult> DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await http.DeleteAsync($"/api/usuarios/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent)
            return UsuarioResult.Ok();

        if (response.StatusCode == HttpStatusCode.NotFound)
            return UsuarioResult.Fail("Usuario nao encontrado.");

        return UsuarioResult.Fail("Nao foi possivel excluir o usuario agora. Tente novamente em instantes.");
    }
}
