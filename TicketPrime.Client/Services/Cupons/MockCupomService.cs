namespace TicketPrime.Client.Services.Cupons;

public sealed class MockCupomService : ICupomService
{
    private static readonly List<CupomListItem> Cupons =
    [
        new CupomListItem { Id = 1, Codigo = "BEMVINDO10", PorcentagemDesconto = 10m, ValorMinimoRegra = 50m },
        new CupomListItem { Id = 2, Codigo = "FESTA20", PorcentagemDesconto = 20m, ValorMinimoRegra = 120m }
    ];

    public Task<CriarCupomResult> CriarAsync(CriarCupomRequest request, CancellationToken cancellationToken = default)
    {
        var codigo = request.Codigo.Trim();

        if (Cupons.Any(c => string.Equals(c.Codigo, codigo, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.FromResult(CriarCupomResult.Fail("Codigo de cupom ja cadastrado."));
        }

        Cupons.Add(new CupomListItem
        {
            Id = Cupons.Count == 0 ? 1 : Cupons.Max(c => c.Id) + 1,
            Codigo = codigo,
            PorcentagemDesconto = request.PorcentagemDesconto,
            ValorMinimoRegra = request.ValorMinimoRegra
        });

        return Task.FromResult(CriarCupomResult.Ok());
    }

    public Task<IReadOnlyList<CupomListItem>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var ordered = Cupons
            .OrderBy(c => c.Codigo, StringComparer.OrdinalIgnoreCase)
            .Select(c => new CupomListItem
            {
                Id = c.Id,
                Codigo = c.Codigo,
                PorcentagemDesconto = c.PorcentagemDesconto,
                ValorMinimoRegra = c.ValorMinimoRegra
            })
            .ToList();

        return Task.FromResult<IReadOnlyList<CupomListItem>>(ordered);
    }

    public Task<CupomListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var found = Cupons.FirstOrDefault(c => c.Id == id);

        if (found is null)
        {
            return Task.FromResult<CupomListItem?>(null);
        }

        return Task.FromResult<CupomListItem?>(new CupomListItem
        {
            Id = found.Id,
            Codigo = found.Codigo,
            PorcentagemDesconto = found.PorcentagemDesconto,
            ValorMinimoRegra = found.ValorMinimoRegra
        });
    }

    public Task<CriarCupomResult> AtualizarAsync(int id, AtualizarCupomRequest request, CancellationToken cancellationToken = default)
    {
        var codigoNovo = request.Codigo.Trim();

        var cupomExistente = Cupons.FirstOrDefault(c => c.Id == id);

        if (cupomExistente is null)
        {
            return Task.FromResult(CriarCupomResult.Fail("Cupom nao encontrado para edicao."));
        }

        var codigoDuplicado = Cupons.Any(c =>
            c.Id != id &&
            string.Equals(c.Codigo, codigoNovo, StringComparison.OrdinalIgnoreCase));

        if (codigoDuplicado)
        {
            return Task.FromResult(CriarCupomResult.Fail("Codigo de cupom ja cadastrado."));
        }

        cupomExistente.Codigo = codigoNovo;
        cupomExistente.PorcentagemDesconto = request.PorcentagemDesconto;
        cupomExistente.ValorMinimoRegra = request.ValorMinimoRegra;

        return Task.FromResult(CriarCupomResult.Ok());
    }

    public Task<CriarCupomResult> DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var removed = Cupons.RemoveAll(c => c.Id == id);

        return removed > 0
            ? Task.FromResult(CriarCupomResult.Ok())
            : Task.FromResult(CriarCupomResult.Fail("Cupom nao encontrado."));
    }
}
