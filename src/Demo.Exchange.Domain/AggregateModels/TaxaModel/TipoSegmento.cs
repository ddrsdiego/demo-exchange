namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using Demo.Exchange.Domain.SeedWorks;
    using System;
    using System.Linq;

    public class TipoSegmento : Enumeration
    {
        public TipoSegmento(string id, string name, string desricaoSimples)
            : base(id, name)
        {
            DesricaoSimples = desricaoSimples;
        }

        public static readonly TipoSegmento Varejo = new TipoSegmento("VAREJO", "Segmento para cliente do varejo", nameof(Varejo));
        public static readonly TipoSegmento Personnalite = new TipoSegmento("PERSONNALITE", "Segmento para cliente do Personnalite", nameof(Personnalite));
        public static readonly TipoSegmento Private = new TipoSegmento("PRIVATE", "Segmento para cliente do Private", nameof(Private));

        public string DesricaoSimples { get; }

        private static readonly TipoSegmento[] TipoSegmentos = new TipoSegmento[] { Varejo, Personnalite, Private };

        public static implicit operator TipoSegmento(string id)
        {
            var tipoSegmento = ObterPorId(id);
            if (tipoSegmento is null)
                throw new ArgumentException(nameof(id));

            return tipoSegmento;
        }

        public static TipoSegmento ObterPorId(string id) => GetAll<TipoSegmento>().SingleOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        public static TipoSegmento ObterPorIdFromArray(string id)
            => TipoSegmentos.SingleOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        public static TipoSegmento ObterTipoSegmentoVarejo() => Varejo;

        public static TipoSegmento ObterPorIdFor(string id)
        {
            for (int i = 0; i < TipoSegmentos.Length; i++)
            {
                if (TipoSegmentos[i].Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                    return TipoSegmentos[i];
            }

            return null;
        }

        public static TipoSegmento ObterPorIdForSemInvariantCultureIgnoreCase(string id)
        {
            for (int i = 0; i < TipoSegmentos.Length; i++)
            {
                if (TipoSegmentos[i].Id.ToLowerInvariant().Equals(id.ToLowerInvariant()))
                    return TipoSegmentos[i];
            }

            return null;
        }

        public static TipoSegmento ObterPorIdIf(string id)
        {
            if (Varejo.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)) return Varejo;

            if (Personnalite.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)) return Personnalite;

            if (Private.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)) return Private;

            return null;
        }
    }
}