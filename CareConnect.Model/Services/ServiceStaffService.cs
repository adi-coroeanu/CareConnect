using CareConnect.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class ServiceStaffService
    {
        private readonly ModelContext _modelContext;
        private readonly AuditService _auditService;

        public ServiceStaffService(ModelContext modelContext, AuditService auditService)
        {
            _modelContext = modelContext;
            _auditService = auditService;
        }

        public void DeleteService(Service selectedService)
        {
            _modelContext.Services.Remove(selectedService);

            _modelContext.SaveChanges();
        }

        public void CreateService(string staffId, string serviceName, string estTimeMinutes, DateTime timeStart, DateTime timeEnd, string price)
        {
            var timeStartString = timeStart.TimeOfDay.ToString(@"hh\:mm");
            var timeEndString = timeEnd.TimeOfDay.ToString(@"hh\:mm");
            var minutesInt = int.Parse(estTimeMinutes);
            var priceDecimal = decimal.Parse(price);

            var newService = new Service
            {
                Id = Guid.NewGuid().ToString(),
                IdDoctor = staffId,
                Name = serviceName,
                EstTimeMinutes = minutesInt,
                TimeStart = timeStartString,
                TimeEnd = timeEndString,
                Price = priceDecimal
            };

            _modelContext.Services.Add(newService);

            _modelContext.SaveChanges();

            _auditService.Log($"Created a new service [ServiceId: {newService.Id}]", staffId);
        } 

        public List<Service> GetStaffServiceList(string staffId)
        {
            var staffServiceList = _modelContext.Services.Where(s => s.IdDoctor == staffId).ToList();

            return staffServiceList;
        }
    }
}
