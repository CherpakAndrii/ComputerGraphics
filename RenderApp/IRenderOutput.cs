using Core.Lights;

namespace RenderApp;

public interface IRenderOutput
{
    public void CreateRenderResult(Color[,] pixels);
}