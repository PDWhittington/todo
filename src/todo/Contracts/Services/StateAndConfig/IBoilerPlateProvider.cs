using System;
using System.Text;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IBoilerPlateProvider
{
    void MakeBoilerPlate(StringBuilder sb);

    string GetBoilerPlate();
}
