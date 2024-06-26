using PecuarioProPlatform.API.BusinessAdministration.Domain.Model.Commands;

namespace PecuarioProPlatform.API.BusinessAdministration.Domain.Model.Aggregates;

public partial class Campaign
{
    public int Id { get; }
    public string Name { get; private set; }
    public DateOnly DateStart { get; private set; }
    public DateOnly DateEnd { get; private set; }
    public string Objective { get; private set; }

    public Campaign(string _Name, DateOnly _DateStart, DateOnly? _DateEnd, string _Objective) : this()
    {
        Name = _Name;
        DateStart = _DateStart;
        DateEnd = _DateEnd ?? DateStart;
        Objective = _Objective;
        CalculateDuration();
    }
    
    
    
    public Campaign(CreateCampaignCommand command) : this(command.name, command.dateStart, command. dateEnd,command. objective){}
    
}