using System;
using System.Diagnostics;
using Downtime.Alerter.DAL.Interface;
using Downtime.Alerter.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Downtime.Alerter.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Downtime.Alerter.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITargetApplicationService _service;
        private readonly IMonitoringHistoryService _monitorService;
        private UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ITargetApplicationService service, UserManager<IdentityUser> userManager, IMonitoringHistoryService monitorService)
        {
            _logger = logger;
            _service = service;
            _userManager = userManager;
            _monitorService = monitorService;

            _logger.Log(LogLevel.Information, "test");
        }

        public IActionResult Index()
        {
            try
            {
                var targetApplications = _service.GetTargetApplicationsOfUser(_userManager.GetUserId(User));
                return View(targetApplications);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
            
        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var targetApplication = _service.GeTargetApplicationById(id.Value, _userManager.GetUserId(User));
                if (targetApplication == null)
                {
                    return NotFound();
                }

                return View(targetApplication);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult MonitorDetails(long? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var monitoringHistory = _monitorService.GetMonitoringHistoryDetail(id.Value, _userManager.GetUserId(User));
                if (monitoringHistory == null)
                {
                    return NotFound();
                }

                return View(monitoringHistory);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Url,Interval,NotificationTypes")] TargetApplicationVm targetApplication)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    targetApplication.UserId = _userManager.GetUserId(User);
                    _service.AddEditTargetApplication(targetApplication);
                    return RedirectToAction(nameof(Index));
                }
                return View(targetApplication);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
            
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var targetApplication = _service.GeTargetApplicationById(id.Value, _userManager.GetUserId(User));
                if (targetApplication == null)
                {
                    return NotFound();
                }
                return View(targetApplication);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Url,Interval,NotificationTypes")] TargetApplicationVm targetApplication)
        {
            try
            {
                if (id != targetApplication.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        targetApplication.UserId = _userManager.GetUserId(User);
                        _service.AddEditTargetApplication(targetApplication);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Error, ex, "An error occured");
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(targetApplication);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
            
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var targetApplication = _service.GeTargetApplicationById(id.Value, _userManager.GetUserId(User));
                if (targetApplication == null)
                {
                    return NotFound();
                }

                return View(targetApplication);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
            
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.DeleteTargetApplicationById(id, _userManager.GetUserId(User));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return RedirectToAction(nameof(Error));
            }
            
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
