using System.Collections.Generic;

namespace BoardRepositories.Interfaces
{
    public delegate void ActionParametersResetHandler(object sender, IDictionary<string, string> newParameters);
    public interface IBoardParamsNotification
    {
        event ActionParametersResetHandler ParametersChanged;
        IDictionary<string, string> Parameters { get; }

        bool SetParameters(IDictionary<string,string> parameters);
    }
}
