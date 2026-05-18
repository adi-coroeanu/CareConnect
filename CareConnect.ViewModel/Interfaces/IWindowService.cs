using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.ViewModel.Interfaces
{
    public interface IWindowService
    {
        public void OpenWindow<TWindow>() where TWindow : class;

        public void CloseWindow<TWindow>()where TWindow : class;
    }
}
