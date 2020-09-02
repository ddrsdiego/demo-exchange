namespace Demo.Exchange.UnitTest.FakesData
{
    using Demo.Exchange.Application.Models;
    using System;

    public static partial class FakeData
    {
        public static TaxaResponse TaxaResponseValid =>
            new TaxaResponse
            {
                Id = "53d2dcad-139b-441f-8bbb-2f3bab085819",
                TipoSegmento = "PERSONNALITE",
                ValorTaxa = 10.00M,
                CriadoEm = DateTime.Today
            };
    }
}