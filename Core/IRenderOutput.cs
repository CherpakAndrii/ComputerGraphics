using Core.Lights;

namespace Core;

public interface IRenderOutput
{
    public void CreateRenderResult(Color[,] pixels);
}