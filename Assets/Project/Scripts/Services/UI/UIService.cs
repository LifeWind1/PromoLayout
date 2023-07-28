using Grace.DependencyInjection;
using RedPanda.Project.Services.Interfaces;
using UnityEngine;

namespace RedPanda.Project.Services.UI
{
    public sealed class UIService : IUIService
    {
        private readonly IExportLocatorScope _container;
        private readonly GameObject _canvas;
        private UIControl _viewControl;


        public UIService(IExportLocatorScope container)
        {
            _container = container;
            _canvas = Object.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
            _canvas.name = "Canvas";
        }

        void IUIService.Show(string viewName)
        {
            _viewControl = new UIControl(viewName, _canvas, _container);
        }

        public void Close(string viewName)
        {
            _viewControl?.Close();
        }
    }
}