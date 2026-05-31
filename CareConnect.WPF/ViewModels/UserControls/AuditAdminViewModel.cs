using CareConnect.Model.Services;
using CareConnect.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CareConnect.WPF.ViewModels.UserControls
{
    public class AuditAdminViewModel : NotifyPropertyService
    {
        private readonly AuditAdminService _auditAdminService;

        private string? _selectedAuditDate;

        public ObservableCollection<string> AuditList { get; }

        public AuditAdminViewModel(AuditAdminService auditAdminService)
        {
            _auditAdminService = auditAdminService;

            AuditList = new ObservableCollection<string>(_auditAdminService.GetAuditList());
        }

        public string? SelectedAuditDate
        {
            get => _selectedAuditDate;
            set
            {
                _selectedAuditDate = value;

                Process.Start("notepad.exe", Path.Combine(_auditAdminService.GetAuditPath, string.Concat(_selectedAuditDate, ".txt")));

                OnPropertyChanged();
            }
        }
    }
}
