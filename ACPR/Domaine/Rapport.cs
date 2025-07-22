using System.Text.Json.Serialization;
using ACPR.Helper;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;

namespace ACPR.Domaine
{
    public class Rapport
    {
        [JsonPropertyName("incidentSubmission")]
        public string? TypeRapportIncident { get; private set;} //champ 1.1
        [JsonPropertyName("reportCurrency")]
        public string? DeviseRapport { get; private set;} //champ 1.15; Format ISO 4217
        [JsonPropertyName("submittingEntity")]
        public Entite? EntiteRapport { get; private set;}
        [JsonPropertyName("affectedEntity")]
        public IEnumerable<Entite?>? EntiteAffecte {  get; private set;}
        [JsonPropertyName("ultimateParentUndertaking")]
        public Entite? ParentEntite { get; private set;}
        [JsonPropertyName("primaryContact")]
        public Contact? PremierContact { get; private set;}
        [JsonPropertyName("secondaryContact")]
        public Contact? SecondContact { get; private set;}
        [JsonPropertyName("incident")]
        public Incident? Incident { get; private set; }
        [JsonPropertyName("impactAssessment")]
        public EvaluationImpact? EvaluationImpact { get; private set;}
        [JsonPropertyName("reportingToOtherAuthorities")]
        public IEnumerable<string>? RapportAutorites { get; private set;} //champ 3.31
        [JsonPropertyName("reportingToOtherAuthoritiesOther")]
        public string? RapportAutoriteAutres {  get; private set;} //champ 3.32
        [JsonPropertyName("informationDurationServiceDowntimeActualOrEstimate")]
        public string? DureeActuelOuEstime { get; private set;} //champ 3.17


        public Rapport(string? typeRapport, string? devise, Entite? entiteSoumettant, IEnumerable<Entite?>? entitesAffectes, Entite? entiteParent,
                        Contact? contact1er, Contact? contact2nd, Incident? incident, EvaluationImpact? evaluation, IEnumerable<string>? rapportAutorite,
                        string? autreAutorite, string? duree)
        {
            TypeRapportIncident = typeRapport;
            DeviseRapport = devise;
            EntiteRapport = entiteSoumettant;
            EntiteAffecte = entitesAffectes;
            ParentEntite = entiteParent;
            PremierContact = contact1er;
            SecondContact = contact2nd;
            Incident = incident;
            EvaluationImpact = evaluation;
            RapportAutorites = rapportAutorite;
            RapportAutoriteAutres = autreAutorite;
            DureeActuelOuEstime = duree;
            LoggerHelper.Instance.Log(Resource.RapportReussite, LogLevel.Information);
        }

    }
}
