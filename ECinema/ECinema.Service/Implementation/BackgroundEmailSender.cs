using ECinema.Domain.DomainModels;
using ECinema.Repository.Interface;
using ECinema.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECinema.Service.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {

        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }
        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.Status).ToList());
            var items = _mailRepository.GetAll().Where(z => !z.Status).ToList();

            foreach(EmailMessage item in items)
            {
                item.Status = true;
                _mailRepository.Update(item);
            }
        }
    }
}
