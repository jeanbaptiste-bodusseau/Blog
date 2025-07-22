using ACPR.Domaine;

namespace ACPR.Services;
public interface IModeleService
{
    Dictionary<string, IEnumerable<string>> ValeursExcel { get; set; }
    Rapport CreerRapport();
}
