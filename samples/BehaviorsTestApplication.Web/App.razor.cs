using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace BehaviorsTestApplication.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<BehaviorsTestApplication.App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime();
    }
}
