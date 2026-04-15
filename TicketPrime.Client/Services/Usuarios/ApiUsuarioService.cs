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

    public async Task<UsuarioListItem?> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        var response = await http.GetAsync($"/api/usuarios/{cpf}", cancellationToken);

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

        return UsuarioResult.Fail("Não foi possível cadastrar o usuário agora. Tente novamente em instantes.");
    }

    public async Task<UsuarioResult> AtualizarAsync(string cpf, SalvarUsuarioRequest request, CancellationToken cancellationToken = default)
    {
        var response = await http.PutAsJsonAsync($"/api/usuarios/{cpf}", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
            return UsuarioResult.Ok();

        if (response.StatusCode == HttpStatusCode.NotFound)
            return UsuarioResult.Fail("Usuário não encontrado.");

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return UsuarioResult.Fail(await response.Content.ReadAsStringAsync(cancellationToken));

        return UsuarioResult.Fail("Não foi possível atualizar o usuário agora. Tente novamente em instantes.");
    }

    public async Task<UsuarioResult> DeletarAsync(string cpf, CancellationToken cancellationToken = default)
    {
        var response = await http.DeleteAsync($"/api/usuarios/{cpf}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NoContent)
            return UsuarioResult.Ok();

        if (response.StatusCode == HttpStatusCode.NotFound)
            return UsuarioResult.Fail("Usuário não encontrado.");

        return UsuarioResult.Fail("Não foi possível excluir o usuário agora. Tente novamente em instantes.");
    }
}
