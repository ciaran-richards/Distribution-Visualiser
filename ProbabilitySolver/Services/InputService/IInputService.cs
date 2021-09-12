using CSharpFunctionalExtensions;

namespace ProbabilitySolver.Services.InputService
{
    public interface IInputService<Tmedium, Tend>
    {
        Result<Tend> Run(string input);

        Result<Tmedium> Parse(string input);

        Result<Tend> Convert(Tmedium input);
    }
}
