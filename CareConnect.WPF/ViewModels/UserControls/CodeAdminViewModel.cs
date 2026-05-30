using CareConnect.Model.Services;
using CareConnect.ViewModel.Commands;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class CodeAdminViewModel : NotifyPropertyService
    {
        private readonly CodeAdminService _codeAdminService;
        private readonly ActiveUserService _activeUserService;

        private string? _codeText;
        
        public ICommand GenerateCodeCommand { get; }

        public CodeAdminViewModel(CodeAdminService codeAdminService, ActiveUserService activeUserService)
        {
            _codeAdminService = codeAdminService;
            _activeUserService = activeUserService;

            GenerateCodeCommand = new RelayCommand(GenerateCode, CanGenerateCode);
            ExistingCodeError = false;
            CodeText = null;
        }

        public bool ExistingCodeError { get; set; }

        public string? CodeText
        {
            get => _codeText;
            set
            {
                _codeText = value;

                OnPropertyChanged();
            }
        }

        private void GenerateCode(object? parameter)
        {
            if(_codeAdminService.ExistingAdminCode(_activeUserService.ActiveUser!.Id))
            {
                ExistingCodeError = true;
                OnPropertyChanged(nameof(ExistingCodeError));
                ((RelayCommand)GenerateCodeCommand).Refresh();
                return;
            }

            CodeText = _codeAdminService.GenerateCode(_activeUserService.ActiveUser!.Id);

            MessageBox.Show("The code can be used for 5 minutes.");
        }

        private bool CanGenerateCode(object? parameter)
        {
            if (ExistingCodeError)
                return false;
            return true;
        }
    }
}
